using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Display(Name = "Benutzername")]
        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Vorname muss ausgefüllt sein")]
            [Display(Name = "Vorname")]
            public string FirstName { get; set; }

            [Display(Name = "Nachname")]
            [Required(ErrorMessage = "Nachname muss ausgefüllt sein")]
            public string LastName { get; set; }

            [Display(Name = "Event-Newsletter")]
            public bool SendEventNewsletter { get; set; }

            [Display(Name = "Kontaktdaten")]
            public string ContactData { get; set; }

            public string ProfilePicture { get; set; }
        }

        public async Task OnGetSaveImageAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                SendEventNewsletter = user.SendEventNewsletter,
                ProfilePicture = user.ProfilePicture,
                ContactData = user.ContactData
            };

            if (user.TemporaryProfilePicture == null)
            {
                return;
            }

            user.ProfilePicture = user.TemporaryProfilePicture;
            Input.ProfilePicture = user.ProfilePicture;
            user.TemporaryProfilePicture = null;
            await _userManager.UpdateAsync(user);
            await _userManager.GetUserAsync(User);
        }

        private void Load(ApplicationUser user)
        {
            Input = new InputModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                SendEventNewsletter = user.SendEventNewsletter,
                ProfilePicture = user.ProfilePicture,
                ContactData = user.ContactData
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Load(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                Load(user);
                return Page();
            }

            user.FirstName = Input.FirstName;
            user.LastName = Input.LastName;
            user.FullName = Input.FirstName + " " + Input.LastName;
            user.SendEventNewsletter = Input.SendEventNewsletter;
            user.ContactData = Input.ContactData;
            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Dein Profil wurde gespeichert";
            return RedirectToPage();
        }
    }
}
