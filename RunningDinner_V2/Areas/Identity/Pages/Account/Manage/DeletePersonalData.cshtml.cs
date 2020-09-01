using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RunningDinner.Data;
using RunningDinner.Extensions;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly IEmailSender _emailSender;
        IEventParticipationsRepository EventParticipationsRepository { get; set; }
        ITeamsRepository TeamsRepository { get; set; }
        IRoutesRepository RoutesRepository { get; set; }
        IInvitationsRepository InvitationsRepository { get; set; }
        IConfiguration Configuration { get; set; }

        public DeletePersonalDataModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            IEventParticipationsRepository eventParticipationsRepository,
            IEmailSender emailSender,
            IRoutesRepository routesRepository,
            IConfiguration configuration,
            ITeamsRepository teamsRepository,
            IInvitationsRepository invitationsRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            Configuration = configuration;
            _emailSender = emailSender;
            RoutesRepository = routesRepository;
            EventParticipationsRepository = eventParticipationsRepository;
            TeamsRepository = teamsRepository;
            InvitationsRepository = invitationsRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Passwort muss ausgefüllt sein")]
            [DataType(DataType.Password)]
            [Display(Name = "Passwort")]
            public string Password { get; set; }
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            // Remove contact from contact list
            string apiKey = Configuration?.GetEmailSettings("apiKey");
            string apiSecret = Configuration?.GetEmailSettings("apiSecret");
            Team team = TeamsRepository.SearchFor(x => x.Partner1.Id == user.Id || x.Partner2.Id == user.Id).Include("Partner1").Include("Partner2").FirstOrDefault();
            if (team != null)
            {
                // Delete from existing routes
                var routes = RoutesRepository.SearchFor(x =>
                x.RouteForTeam.Id == team.Id ||
                x.FirstCourseHostTeam.Id == team.Id ||
                x.FirstCourseGuestTeam1.Id == team.Id ||
                x.FirstCourseGuestTeam2.Id == team.Id ||
                x.SecondCourseHostTeam.Id == team.Id ||
                x.SecondCourseGuestTeam1.Id == team.Id ||
                x.SecondCourseGuestTeam2.Id == team.Id ||
                x.ThirdCourseHostTeam.Id == team.Id ||
                x.ThirdCourseGuestTeam1.Id == team.Id ||
                x.ThirdCourseGuestTeam2.Id == team.Id)
                .Include(a => a.RouteForTeam)
                .Include(a => a.FirstCourseHostTeam)
                .Include(a => a.FirstCourseGuestTeam1)
                .Include(a => a.FirstCourseGuestTeam2)
                .Include(a => a.SecondCourseHostTeam)
                .Include(a => a.SecondCourseGuestTeam1)
                .Include(a => a.SecondCourseGuestTeam2)
                .Include(a => a.ThirdCourseHostTeam)
                .Include(a => a.ThirdCourseGuestTeam1)
                .Include(a => a.ThirdCourseGuestTeam2);
                foreach (var route in routes)
                {
                    if (route.RouteForTeam.Id == team.Id)
                    {
                        route.RouteForTeam = null;
                    }

                    if (route.FirstCourseHostTeam.Id == team.Id)
                    {
                        route.FirstCourseHostTeam = null;
                    }

                    if (route.FirstCourseGuestTeam1.Id == team.Id)
                    {
                        route.FirstCourseGuestTeam1 = null;
                    }

                    if (route.FirstCourseGuestTeam2.Id == team.Id)
                    {
                        route.FirstCourseGuestTeam2 = null;
                    }

                    if (route.SecondCourseHostTeam.Id == team.Id)
                    {
                        route.SecondCourseHostTeam = null;
                    }

                    if (route.SecondCourseGuestTeam1.Id == team.Id)
                    {
                        route.SecondCourseGuestTeam1 = null;
                    }

                    if (route.SecondCourseGuestTeam2.Id == team.Id)
                    {
                        route.SecondCourseGuestTeam2 = null;
                    }

                    if (route.ThirdCourseHostTeam.Id == team.Id)
                    {
                        route.ThirdCourseHostTeam = null;
                    }

                    if (route.ThirdCourseGuestTeam1.Id == team.Id)
                    {
                        route.ThirdCourseGuestTeam1 = null;
                    }

                    if (route.ThirdCourseGuestTeam2.Id == team.Id)
                    {
                        route.ThirdCourseGuestTeam2 = null;
                    }

                    RoutesRepository.Update(route);
                }

                RoutesRepository.SaveChanges();

                if (team.Partner1.Id == user.Id || team.Partner2.Id == user.Id)
                {
                    if (team.Partner1 != null)
                    {
                        await _emailSender.RemoveListRecipientAsync(apiKey, apiSecret, team.Partner1.ListRecipientId);
                    }

                    if (team.Partner2 != null)
                    {
                        await _emailSender.RemoveListRecipientAsync(apiKey, apiSecret, team.Partner2.ListRecipientId);
                    }

                    TeamsRepository.Delete(team);
                    TeamsRepository.SaveChanges();
                }
            }

            // Delete participations
            var participations = EventParticipationsRepository.SearchFor(x => x.User.Id == user.Id).ToList();
            foreach (var participation in participations)
            {
                EventParticipationsRepository.Delete(participation);
                EventParticipationsRepository.SaveChanges();
            }

            // Delete invitations
            var invitations = InvitationsRepository.SearchFor(x => x.User.Id == user.Id).ToList();
            foreach (var invitation in invitations)
            {
                InvitationsRepository.Delete(invitation);
                InvitationsRepository.SaveChanges();
            }

            // Delete user from Mailjet contact lists
            await _emailSender.RemoveListRecipientAsync(apiKey, apiSecret, user.ListRecipientId);

            // Delete user
            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);
            return Redirect("~/");
        }
    }
}
