using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        SignInManager<ApplicationUser> SignInManager;

        public RegisterConfirmationModel(SignInManager<ApplicationUser> signInManager)
        {
            SignInManager = signInManager;
        }

        public string Email { get; set; }

        public bool DisplayConfirmAccountLink { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
