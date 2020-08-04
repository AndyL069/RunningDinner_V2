using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
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
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private IConfiguration Configuration { get; set; }

        public ExternalLoginModel(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            ILogger<ExternalLoginModel> logger,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            Configuration = configuration;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string LoginProvider { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            public string ProfilePicture { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl ??= Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }

            var info = await _signInManager.GetExternalLoginInfoAsync().ConfigureAwait(false);
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor : true).ConfigureAwait(false);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            if (result.IsNotAllowed)
            {
                _logger.LogWarning("User not allowed.");
                return RedirectToPage("RegisterConfirmation");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                LoginProvider = info.LoginProvider;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    var nameSplitted = info.Principal.Identity.Name.Split(" ");
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        ProfilePicture = info.Principal.FindFirstValue(JwtClaimTypes.Picture),
                        FirstName = nameSplitted[0],
                        LastName = nameSplitted[1]
                    };
                }

                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser {
                        UserName = Input.Email,
                        Email = Input.Email,
                        SendEventNewsletter = true,
                        ProfilePicture = Input.ProfilePicture,
                        FirstName = Input.FirstName, 
                        LastName = Input.LastName };
                    var createResult = await _userManager.CreateAsync(user).ConfigureAwait(false);
                    if (createResult.Succeeded)
                    {
                        createResult = await _userManager.AddLoginAsync(user, info).ConfigureAwait(false);
                        if (createResult.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
                            _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                            var userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
                            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                            var callbackUrl = Url.Page(
                                "/Account/ConfirmEmail",
                                pageHandler: null,
                                values: new { area = "Identity", userId, code },
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
                            return LocalRedirect(returnUrl);
                        }
                    }

                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync().ConfigureAwait(false);
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = Input.Email, 
                    Email = Input.Email,
                    ProfilePicture = Input.ProfilePicture,
                    FirstName = Input.FirstName, 
                    LastName = Input.LastName };
                var result = await _userManager.CreateAsync(user).ConfigureAwait(false);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false).ConfigureAwait(false);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        var userId = await _userManager.GetUserIdAsync(user).ConfigureAwait(false);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId, code },
                            protocol: Request.Scheme);

                        // Send the email
                        string apiKey = Configuration?.GetEmailSettings("apiKey");
                        string apiSecret = Configuration?.GetEmailSettings("apiSecret");
                        await _emailSender.SendMailjetAsync(apiKey, apiSecret, 1080920, "Bitte bestätige deine Emailadresse", "admin@grossstadtdinner.de", "Das Großstadt Dinner Team", Input.FirstName, Input.Email, Input.FirstName + " " + Input.LastName, callbackUrl).ConfigureAwait(false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            LoginProvider = info.LoginProvider;
            ReturnUrl = returnUrl;
            return Page();
        }
    }
}
