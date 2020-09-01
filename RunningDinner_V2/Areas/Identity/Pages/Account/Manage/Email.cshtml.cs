using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using RunningDinner.Extensions;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Identity.Pages.Account.Manage
{
    public partial class EmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;
        private IConfiguration Configuration { get; set; }

        public EmailModel(
            UserManager<ApplicationUser> userManager,
            IEmailSender emailSender,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            Configuration = configuration;
        }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Neue E-Mail muss ausgefüllt sein")]
            [EmailAddress]
            [Display(Name = "Neue E-Mail")]
            public string NewEmail { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var email = await _userManager.GetEmailAsync(user);
            Email = email;
            Input = new InputModel
            {
                NewEmail = email,
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostChangeEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId, email = Input.NewEmail, code },
                    protocol: Request.Scheme);
                // Send the email
                string apiKey = Configuration?.GetEmailSettings("apiKey");
                string apiSecret = Configuration?.GetEmailSettings("apiSecret");
                await _emailSender.SendMailjetAsync(apiKey, apiSecret, 1080920, "Bitte bestätige deine Emailadresse", "admin@grossstadtdinner.de", "Das Großstadt Dinner Team", user.FirstName, user.Email, user.FirstName + " " + user.LastName, callbackUrl);
                StatusMessage = "Eine E-Mail mit Bestätigungslink wurde verschickt. Bitte überprüfe deine E-Mails.";
                return RedirectToPage();
            }

            StatusMessage = "Deine E-Mail-Adresse hat sich nicht geändert.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId, code },
                protocol: Request.Scheme);
            // Send the email
            string apiKey = Configuration?.GetEmailSettings("apiKey");
            string apiSecret = Configuration?.GetEmailSettings("apiSecret");
            await _emailSender.SendMailjetAsync(apiKey, apiSecret, 1080920, "Bitte bestätige deine Emailadresse", "admin@grossstadtdinner.de", "Das Großstadt Dinner Team", user.FirstName, user.Email, user.FirstName + " " + user.LastName, callbackUrl);
            StatusMessage = "Bestätigungslink wurde verschickt. Bitte schaue in deinen E-Mails nach.";
            return RedirectToPage();
        }
    }
}
