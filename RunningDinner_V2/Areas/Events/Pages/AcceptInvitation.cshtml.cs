using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RunningDinner.Data;
using RunningDinner.Extensions;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Events.Pages
{
    [AllowAnonymous]
    public class AcceptInvitationModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private IEventParticipationsRepository EventParticipationsRepository { get; set; }

        private IDinnerEventsRepository DinnerEventsRepository { get; set; }

        private ITeamsRepository TeamsRepository { get; set; }

        private IInvitationsRepository InvitationsRepository { get; set; }

        private IConfiguration Configuration { get; set; }

        private IEmailSender EmailSender { get; set; }

        public AcceptInvitationModel(UserManager<ApplicationUser> userManager,
            IEventParticipationsRepository eventParticipationsRepository,
            ITeamsRepository teamsRepository,
            IInvitationsRepository invitationsRepository,
            IDinnerEventsRepository dinnerEventsRepository,
            IConfiguration configuration,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            EventParticipationsRepository = eventParticipationsRepository;
            TeamsRepository = teamsRepository;
            DinnerEventsRepository = dinnerEventsRepository;
            InvitationsRepository = invitationsRepository;
            Configuration = configuration;
            EmailSender = emailSender;
        }

        public async Task<IActionResult> OnGet(int invitationId, string email, string returnUrl = null)
        {
            // Return if invitation is already accepted
            Invitation invitation = InvitationsRepository.SearchFor(x => x.Id == invitationId).Include("User").Include("Event").FirstOrDefault();
            if (invitation.InvitationAccepted)
            {
                // Display page?
                return RedirectToAction("Index", "Home");
            }

            ApplicationUser invitee = await _userManager.FindByEmailAsync(email);
            if (invitee == null)
            {
                // If user doesn't exist, redirect to Register page
                return RedirectToPage("/Account/Register", new { area = "Identity", returnUrl });
            }

            DinnerEvent dinnerEvent = DinnerEventsRepository.GetById(invitation.Event.Id);
            EventParticipation inviteeParticipation = EventParticipationsRepository.SearchFor(x => x.User.Id == invitee.Id && x.Event.Id == invitation.Event.Id).FirstOrDefault();
            if (inviteeParticipation == null)
            {
                inviteeParticipation = new EventParticipation { User = invitee, Event = dinnerEvent };
                EventParticipationsRepository.Insert(inviteeParticipation);
                EventParticipationsRepository.SaveChanges();
            }

            invitation.InvitationAccepted = true;
            InvitationsRepository.Update(invitation);
            // Check if user has more pending invitations and make them invalid
            var otherInvitations = InvitationsRepository.SearchFor(x => x.User.Id == invitee.Id);
            foreach (var otherInvitation in otherInvitations)
            {
                otherInvitation.InvitationAccepted = true;
                InvitationsRepository.Update(otherInvitation);
            }

            InvitationsRepository.SaveChanges();
            // Add contact to Mailjet Contacts, save ContactId
            string apiKey = Configuration?.GetEmailSettings("apiKey");
            string apiSecret = Configuration?.GetEmailSettings("apiSecret");
            // Save list recipient id to database
            long listRecipientId = await EmailSender.AddContactToContactListAsync(apiKey, apiSecret, invitee.ContactId.ToString(CultureInfo.InvariantCulture), dinnerEvent.ContactList.ToString(CultureInfo.InvariantCulture));
            invitee.ListRecipientId = listRecipientId;
            await _userManager.UpdateAsync(invitee);
            EventParticipation inviterParticipation = EventParticipationsRepository.SearchFor(x => x.User.Id == invitation.User.Id && x.Event.Id == invitation.Event.Id).FirstOrDefault();
            // Create a new team
            Team newTeam = new Team
            {
                Partner1 = invitation.User,
                Partner2 = invitee,
                SelectedCourse = inviterParticipation?.SelectedCourse,
                AddressStreet = inviterParticipation.Address,
                AddressNumber = inviterParticipation.AddressNumber,
                FullAddress = inviterParticipation.Address + " " + inviterParticipation.AddressNumber,
                AddressAdditions = inviterParticipation.AddressAdditions,
                Phone = inviterParticipation.Phone,
                DinnerSaver = inviterParticipation.DinnerSaver,
                Event = invitation.Event,
                Allergies = inviterParticipation.Allergies,
                City = dinnerEvent.City
            };

            TeamsRepository.Insert(newTeam);
            TeamsRepository.SaveChanges();
            // Send the email
            await EmailSender.SendMailjetAsync(apiKey, apiSecret, 1197519, "Deine Einladung wurde angenommen", "admin@grossstadtdinner.de", "Das Großstadt Dinner Team", invitation.User.FirstName, invitation.User.Email, invitation.User.FirstName + " " + invitation.User.LastName, "", invitee.FirstName, invitation.Event.EventName, invitation.Event.EventDate.ToShortDateString());
            TempData["StatusMessage"] = @"Du hast die Einladung erfolgreich angenommen. Die Teamdaten deines Partners wurden übernommen. Ihr seid nun vollständig angemeldet.";
            return RedirectToPage("./RegistrationData");
        }
    }
}
