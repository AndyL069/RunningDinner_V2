using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RunningDinner.Extensions;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private IConfiguration Configuration { get; set; }

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            Configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Bitte stimme den allgemeinen Geschäftsbedingungen zu, um fortzusetzen")]
            public bool AcceptTerms { get; set; }

            public bool SendEventNewsletter { get; set; }

            [Required(ErrorMessage = "Vorname ist ein Pflichtfeld")]
            [Display(Name = "Vorname")]
            public string FirstName { get; set; }

            [Required(ErrorMessage = "Nachname ist ein Pflichtfeld")]
            [Display(Name = "Nachname")]
            public string LastName { get; set; }

            [Required(ErrorMessage = "E-Mail ist ein Pflichtfeld")]
            [EmailAddress(ErrorMessage = "Die eingegebene Adresse ist keine gültige E-Mail-Adresse")]
            [Display(Name = "E-Mail")]
            public string Email { get; set; }

            [EmailAddress(ErrorMessage = "Die eingegebene Adresse ist keine gültige E-Mail-Adresse")]
            [Display(Name = "E-Mail bestätigen")]
            [Compare("Email", ErrorMessage = "Die eingegebenen E-Mail-Adressen stimmen nicht überein")]
            public string ConfirmEmail { get; set; }

            [Required(ErrorMessage = "Passwort ist ein Pflichtfeld")]
            [StringLength(100, ErrorMessage = "Das {0} muss mindestens {2} und maximal {1} Zeichen lang sein", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Passwort")]
            public string Password { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, 
                    Email = Input.Email, 
                    FirstName = Input.FirstName,
                    LastName = Input.LastName, 
                    FullName = Input.FirstName + " " + Input.LastName,
                    SendEventNewsletter = Input.SendEventNewsletter };
                var result = await _userManager.CreateAsync(user, Input.Password).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
                    _logger.LogInformation("User created a new account with password.");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code },
                        protocol: Request.Scheme);

                    // Send the email
                    string apiKey = Configuration?.GetEmailSettings("apiKey");
                    string apiSecret = Configuration?.GetEmailSettings("apiSecret");
                    int contactId = await _emailSender.CreateContactAsync(apiKey, apiSecret, user.FirstName + " " + user.LastName, user.Email).ConfigureAwait(false);
                    if (contactId != 0)
                    {
                        user.ContactId = contactId;
                        int listRecipientId = await _emailSender.AddContactToContactListAsync(apiKey, apiSecret, contactId.ToString(CultureInfo.InvariantCulture), "12508").ConfigureAwait(false);
                        user.ListRecipientId = listRecipientId;
                        await _userManager.UpdateAsync(user).ConfigureAwait(false);
                    }

                    await _emailSender.SendMailjetAsync(apiKey, apiSecret, 1080920, "Bitte bestätige deine Emailadresse", "admin@grossstadtdinner.de", "Das Großstadt Dinner Team", Input.FirstName, Input.Email, Input.FirstName + " " + Input.LastName, callbackUrl).ConfigureAwait(false);
                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    //}

                    //else
                    //{
                        await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
                        return LocalRedirect(returnUrl);
                    //}
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
