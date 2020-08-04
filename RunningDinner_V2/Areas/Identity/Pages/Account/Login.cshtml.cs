using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RunningDinner.Models.DatabaseModels;
using Microsoft.Extensions.Configuration;
using RunningDinner.Extensions;

namespace RunningDinner.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly IEmailSender _emailSender;
        private IConfiguration Configuration { get; set; }

        public LoginModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LoginModel> logger,
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            Configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "E-Mail muss ausgefüllt sein")]
            [EmailAddress(ErrorMessage = "Die eingegebene Adresse ist keine gültige E-Mail-Adresse")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Passwort muss ausgefüllt sein")]
            [DataType(DataType.Password)]
            [Display(Name = "Passwort")]
            public string Password { get; set; }

            [Display(Name = "Benutzername speichern")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme).ConfigureAwait(false);
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList();
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList();
            ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                if (result.IsNotAllowed)
                {
                    _logger.LogWarning("User not allowed.");
                    return RedirectToPage("RegisterConfirmation");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Anmeldung fehlgeschlagen.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync().ConfigureAwait(false)).ToList();
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Input.Email).ConfigureAwait(false);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Bestätigungsmail wurde versendet. Bitte schaue in deinen E-Mails nach.");
            }

            var userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code },
                protocol: Request.Scheme);

            string apiKey = Configuration?.GetEmailSettings("apiKey");
            string apiSecret = Configuration?.GetEmailSettings("apiSecret");
            await _emailSender.SendMailjetAsync(apiKey, apiSecret, 1080920, "Bitte bestätige deine Emailadresse", "admin@grossstadtdinner.de", "Das Großstadt Dinner Team", user.FirstName, user.Email, user.FirstName + " " + user.LastName, callbackUrl).ConfigureAwait(false);
            ModelState.AddModelError(string.Empty, "Bestätigungsmail wurde versendet. Bitte schaue in deinen E-Mails nach.");
            return Page();
        }
    }
}
