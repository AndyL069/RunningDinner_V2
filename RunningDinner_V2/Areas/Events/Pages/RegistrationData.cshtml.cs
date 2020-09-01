using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RunningDinner.Data;
using RunningDinner.Extensions;
using RunningDinner.Helpers;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Events.Pages
{
    [ValidateAntiForgeryToken]
    public class RegistrationDataViewModel : PageModel
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        private IDinnerEventsRepository DinnerEventsRepository { get; set; }
        private IEventParticipationsRepository EventParticipationsRepository { get; set; }
        private IRoutesRepository RoutesRepository { get; set; }
        private ITeamsRepository TeamsRepository { get; set; }
        private IEmailSender EmailSender { get; set; }
        private IInvitationsRepository InvitationsRepository { get; set; }
        private IConfiguration Configuration { get; set; }
        public HereMapsHelper HereMapsHelper { get; set; }

        public RegistrationDataViewModel(IDinnerEventsRepository dinnerEventsRepository,
            IEventParticipationsRepository eventParticipationsRepository,
            ITeamsRepository teamsRepository,
            IInvitationsRepository invitationsRepository,
            IRoutesRepository routesRepository,
            IEmailSender emailSender,
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            DinnerEventsRepository = dinnerEventsRepository;
            EventParticipationsRepository = eventParticipationsRepository;
            TeamsRepository = teamsRepository;
            InvitationsRepository = invitationsRepository;
            EmailSender = emailSender;
            RoutesRepository = routesRepository;
            Configuration = configuration;
            UserManager = userManager;
            SignInManager = signInManager;
            HereMapsHelper = new HereMapsHelper(configuration);
        }

        public bool IsParticipating { get; set; }

        public Team Team { get; set; }

        public DinnerEvent Event { get; set; }

        public bool RoutesOpen { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Vorname")]
            public string FirstName { get; set; }

            [Display(Name = "Nachname")]
            public string LastName { get; set; }

            [Display(Name = "E-Mail")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Vorname muss ausgefüllt sein")]
            [Display(Name = "Partner Vorname")]
            public string PartnerFirstName { get; set; }

            [Required(ErrorMessage = "Nachname muss ausgefüllt sein")]
            [Display(Name = "Partner Nachname")]
            public string PartnerLastName { get; set; }

            [Required(ErrorMessage = "E-Mail muss ausgefüllt sein")]
            [Display(Name = "Partner Email")]
            public string PartnerEmail { get; set; }

            [Display(Name = "Ohne Partner")]
            public bool IsWithoutPartner { get; set; }

            [Required(ErrorMessage = "Straße muss ausgefüllt sein")]
            [Display(Name = "Straße")]
            public string AddressStreet { get; set; }

            [Required(ErrorMessage = "Hausnummer muss ausgefüllt sein")]
            [Display(Name = "Hausnr.")]
            public string AddressNumber { get; set; }

            [Display(Name = "Adresszusatz")]
            public string AddressAdditions { get; set; }

            [Required(ErrorMessage = "Telefon muss ausgefüllt sein")]
            [Display(Name = "Telefon")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "Bitte wähle deinen Wunschgang")]
            [Display(Name = "Wunschgang")]
            public string SelectedCourse { get; set; }

            [Display(Name = "Allergien, Unverträglichkeiten, etc.")]
            public string Allergies { get; set; }

            [Display(Name = "Dinnerretter")]
            public bool DinnerSaver { get; set; }
        }

        /// <summary>
        /// Returns the RegistrationData page.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(int eventId)
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            bool isSignedIn = SignInManager.IsSignedIn(User);
            if (!isSignedIn)
            {
                string returnUrl = HttpContext.Request.Path.ToString() + "?eventid=" + eventId;
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
            }

            // Get eventId
            if (eventId == 0)
            {
                if (HttpContext.Session.GetString("EventId") == null)
                {
                    string returnUrl = HttpContext.Request.Path.ToString();
                    return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
                }

                eventId = int.Parse(HttpContext.Session.GetString("EventId"), CultureInfo.CurrentCulture);
            }

            else
            {
                HttpContext.Session.SetString("EventId", eventId.ToString(CultureInfo.CurrentCulture));
            }

            // Get eventTitle
            DinnerEvent dinnerEvent = DinnerEventsRepository.GetById(eventId);
            if (dinnerEvent != null)
            {
                // Values are passed-through to layout page
                ViewData["EventName"] = dinnerEvent.EventName;
                ViewData["EventDate"] = dinnerEvent.EventDate.ToShortDateString();
                ViewData["EventCity"] = dinnerEvent.City;
                ViewData["EventPictureLink"] = dinnerEvent.EventPictureLink;
                ViewData["EventId"] = eventId;
            }

            IsParticipating = IsUserParticipating(eventId, user.Id);
            ViewData["IsParticipating"] = IsParticipating.ToString(CultureInfo.CurrentCulture);
            HttpContext.Session.SetString("IsParticipating", IsParticipating.ToString(CultureInfo.CurrentCulture));
            // Load EventParticipation
            EventParticipation eventParticipation = EventParticipationsRepository.SearchFor(x => x.User.Id == user.Id && x.Event.Id == eventId).FirstOrDefault();
            if (eventParticipation == null)
            {
                eventParticipation = new EventParticipation
                {
                    User = user,
                    Event = dinnerEvent
                };
            }

            Input = new InputModel();

            // Load Team
            Team team = TeamsRepository.SearchFor(x => x.Event.Id == dinnerEvent.Id && (x.Partner1.Id == user.Id || x.Partner2.Id == user.Id)).Include("Partner1").Include("Partner2").Include("Event").FirstOrDefault();
            if (team != null)
            {
                string partnerFirstName = "";
                string partnerLastName = "";
                string partnerEmail = "";
                if (team.Partner1.Id == user.Id)
                {
                    partnerFirstName = team.Partner2.FirstName;
                    partnerLastName = team.Partner2.LastName;
                    partnerEmail = team.Partner2.Email;
                }

                else
                {
                    partnerFirstName = team.Partner1.FirstName;
                    partnerLastName = team.Partner1.LastName;
                    partnerEmail = team.Partner1.Email;
                }

                Team = team;
                Event = team.Event;
                Input.SelectedCourse = team.SelectedCourse;
                Input.FirstName = user.FirstName;
                Input.LastName = user.LastName;
                Input.Email = user.Email;
                Input.Phone = team.Phone;
                Input.Allergies = team.Allergies;
                Input.AddressStreet = team.AddressStreet;
                Input.AddressAdditions = team.AddressAdditions;
                Input.AddressNumber = team.AddressNumber;
                Input.PartnerFirstName = partnerFirstName;
                Input.PartnerLastName = partnerLastName;
                Input.PartnerEmail = partnerEmail;
                Input.DinnerSaver = team.DinnerSaver;
                Input.IsWithoutPartner = eventParticipation.IsWithoutPartner;
            }

            else
            {
                Team = null;
                Event = eventParticipation.Event;
                Input.SelectedCourse = eventParticipation.SelectedCourse;
                Input.FirstName = user.FirstName;
                Input.LastName = user.LastName;
                Input.Email = user.Email;
                Input.Phone = eventParticipation.Phone;
                Input.Allergies = eventParticipation.Allergies;
                Input.AddressStreet = eventParticipation.Address;
                Input.AddressAdditions = eventParticipation.AddressAdditions;
                Input.AddressNumber = eventParticipation.AddressNumber;
                Input.PartnerFirstName = eventParticipation.PartnerName;
                Input.PartnerLastName = eventParticipation.PartnerLastName;
                Input.PartnerEmail = eventParticipation.PartnerEmail;
                Input.DinnerSaver = eventParticipation.DinnerSaver;
                Input.IsWithoutPartner = eventParticipation.IsWithoutPartner;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("EventId") == null)
            {
                string returnUrl = HttpContext.Request.Path.ToString();
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
            }

            ViewData["IsParticipating"] = HttpContext.Session.GetString("IsParticipating");
            int eventId = int.Parse(HttpContext.Session.GetString("EventId"), CultureInfo.CurrentCulture);
            DinnerEvent dinnerEvent = DinnerEventsRepository.GetById(eventId);
            if (dinnerEvent != null)
            {
                // Values are passed-through to layout page
                ViewData["EventName"] = dinnerEvent.EventName;
                ViewData["EventDate"] = dinnerEvent.EventDate.ToShortDateString();
                ViewData["EventCity"] = dinnerEvent.City;
                ViewData["EventPictureLink"] = dinnerEvent.EventPictureLink;
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            // Load Team
            Team team = TeamsRepository.SearchFor(x => x.Event.Id == dinnerEvent.Id && (x.Partner1.Id == user.Id || x.Partner2.Id == user.Id)).Include("Partner1").Include("Partner2").Include("Event").FirstOrDefault();
            if (team != null)
            {
                team.DinnerSaver = Input.DinnerSaver;
                team.SelectedCourse = Input.SelectedCourse;
                team.AddressStreet = Input.AddressStreet;
                team.AddressAdditions = Input.AddressAdditions;
                team.AddressNumber = Input.AddressNumber;
                team.FullAddress = Input.AddressStreet + " " + Input.AddressNumber;
                TeamsRepository.Update(team);
                TeamsRepository.SaveChanges();
                TempData["StatusMessage"] = @"Die Einstellungen für dein Team wurden gespeichert.";
            }

            else
            {
                EventParticipation participation = EventParticipationsRepository.SearchFor(x => x.User.Id == user.Id && x.Event.Id == eventId).FirstOrDefault();
                if (participation == null)
                {
                    participation = new EventParticipation
                    {
                        InvitationMailSent = true,
                        User = user,
                        Event = dinnerEvent,
                        PartnerName = Input.PartnerFirstName,
                        PartnerLastName = Input.PartnerLastName,
                        PartnerEmail = Input.PartnerEmail,
                        Address = Input.AddressStreet,
                        AddressNumber = Input.AddressNumber,
                        AddressAdditions = Input.AddressAdditions,
                        Phone = Input.Phone,
                        Allergies = Input.Allergies,
                        SelectedCourse = Input.SelectedCourse,
                        IsWithoutPartner = Input.IsWithoutPartner,
                        DinnerSaver = Input.DinnerSaver,
                        RegistrationComplete = true
                    };

                    EventParticipationsRepository.Insert(participation);
                    EventParticipationsRepository.SaveChanges();
                    // Add contact to Mailjet Contacts, save ContactId
                    string apiKey = Configuration?.GetEmailSettings("apiKey");
                    string apiSecret = Configuration?.GetEmailSettings("apiSecret");
                    long listRecipientId = await EmailSender.AddContactToContactListAsync(apiKey, apiSecret, user.ContactId.ToString(CultureInfo.InvariantCulture), dinnerEvent.ContactList.ToString(CultureInfo.InvariantCulture));
                    user.ListRecipientId = listRecipientId;
                    await UserManager.UpdateAsync(user);
                }

                else
                {
                    participation.Phone = Input.Phone;
                    participation.Allergies = Input.Allergies;
                    participation.Address = Input.AddressStreet;
                    participation.AddressNumber = Input.AddressNumber;
                    participation.AddressAdditions = Input.AddressAdditions;
                    participation.PartnerName = Input.PartnerFirstName;
                    participation.PartnerLastName = Input.PartnerLastName;
                    participation.PartnerEmail = Input.PartnerEmail;
                    participation.SelectedCourse = Input.SelectedCourse;
                    participation.IsWithoutPartner = Input.IsWithoutPartner;
                    participation.DinnerSaver = Input.DinnerSaver;
                    participation.RegistrationComplete = true;
                    EventParticipationsRepository.Update(participation);
                    EventParticipationsRepository.SaveChanges();
                }

                await OnPostSendPartnerInvitation(eventId, Input.PartnerFirstName, Input.PartnerLastName, Input.PartnerEmail, user.Id);
                TempData["StatusMessage"] = "Deine Anmeldung wurde gespeichert. Eine Einladung wurde an " + Input.PartnerEmail +
                    " geschickt. Die Anmeldung ist vollständig sobald dein Partner bestätigt hat.";
            }

            return RedirectToPage("./RegistrationData", eventId);
        }

        /// <summary>
        /// Checks if a user is participating at an event.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private bool IsUserParticipating(int eventId, string userId)
        {
            bool isParticipating = false;
            var eventParticipation = EventParticipationsRepository.SearchFor(x => x.Event.Id == eventId && x.User.Id == userId);
            if (eventParticipation.Any())
            {
                isParticipating = true;
            }

            return isParticipating;
        }

        /// <summary>
        /// Sends email to partner for confirmation.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="partnerFirstName"></param>
        /// <param name="partnerEmail"></param>
        /// <param name="partnerEmail"></param>
        /// <param name="userId"></param>
        public async Task<IActionResult> OnPostSendPartnerInvitation(int eventId, string partnerFirstName, string partnerLastName, string partnerEmail, string userId)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Page();
            }

            DinnerEvent dinnerEvent = DinnerEventsRepository.GetById(eventId);
            if (dinnerEvent == null)
            {
                return Page();
            }

            Invitation invitation = new Invitation
            {
                Event = dinnerEvent,
                InvitationEmail = partnerEmail,
                User = user,
                SentTime = DateTime.Now
            };

            InvitationsRepository.Insert(invitation);
            InvitationsRepository.SaveChanges();
            EventParticipation participation = EventParticipationsRepository.SearchFor(x => x.User.Id == user.Id && x.Event.Id == eventId).FirstOrDefault();
            if (participation != null)
            {
                if (!participation.RegistrationComplete)
                {
                    TempData["StatusMessage"] = @"Bitte fülle zuerst die Teamdaten aus.";
                    return Page();
                }

                participation.InvitationMailSent = true;
                EventParticipationsRepository.Update(participation);
                EventParticipationsRepository.SaveChanges();
            }

            var confirmationLink = Url.PartnerConfirmationLink(invitation.Id, partnerEmail);
            var callbackLink = Url.PartnerConfirmationLinkCallback(invitation.Id, partnerEmail, confirmationLink, Request.Scheme);
            // Send the email
            string apiKey = Configuration?.GetEmailSettings("apiKey");
            string apiSecret = Configuration?.GetEmailSettings("apiSecret");
            await EmailSender.SendMailjetAsync(apiKey, apiSecret, 1081044, "Du wurdest zum Großstadt Dinner eingeladen", "admin@grossstadtdinner.de", "Das Großstadt Dinner Team", partnerFirstName, partnerEmail, partnerFirstName + " " + partnerLastName, callbackLink, user.FirstName, dinnerEvent.EventName, dinnerEvent.EventDate.ToShortDateString());
            TempData["StatusMessage"] = @"Eine Einladung wurde an " + partnerEmail + " geschickt.";
            return Page();
        }

        /// <summary>
        /// Unregister from event method.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnGetEventUnregister()
        {
            if (HttpContext.Session.GetString("EventId") == null)
            {
                string returnUrl = HttpContext.Request.Path.ToString();
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl });
            }

            var eventId = int.Parse(HttpContext.Session.GetString("EventId"), CultureInfo.CurrentCulture);
            var dinnerEvent = DinnerEventsRepository.GetById(eventId);
            var user = await UserManager.GetUserAsync(HttpContext.User);
            EventParticipation eventParticipation = EventParticipationsRepository.SearchFor(x => x.User == user && x.Event == dinnerEvent).FirstOrDefault();
            EventParticipationsRepository.Delete(eventParticipation);
            EventParticipationsRepository.SaveChanges();
            Team team = TeamsRepository.SearchFor(x => x.Event.Id == dinnerEvent.Id && (x.Partner1.Id == user.Id || x.Partner2.Id == user.Id)).Include("Partner1").Include("Partner2").FirstOrDefault();
            // Remove team
            if (team != null)
            {
                if (dinnerEvent.RoutesOpen)
                {
                    // Delete from existing routes
                    var routes = RoutesRepository.SearchFor(x => x.Event.Id == eventId && x.RouteForTeam.Id == team.Id)
                        .Include(a => a.Event)
                        .Include(a => a.RouteForTeam)
                        .Include(a => a.FirstCourseHostTeam).ThenInclude(b => b.Partner1)
                        .Include(a => a.FirstCourseHostTeam).ThenInclude(b => b.Partner2)
                        .Include(a => a.FirstCourseGuestTeam1).ThenInclude(b => b.Partner1)
                        .Include(a => a.FirstCourseGuestTeam1).ThenInclude(b => b.Partner2)
                        .Include(a => a.FirstCourseGuestTeam2).ThenInclude(b => b.Partner1)
                        .Include(a => a.FirstCourseGuestTeam2).ThenInclude(b => b.Partner2)
                        .Include(a => a.SecondCourseHostTeam).ThenInclude(b => b.Partner1)
                        .Include(a => a.SecondCourseHostTeam).ThenInclude(b => b.Partner2)
                        .Include(a => a.SecondCourseGuestTeam1).ThenInclude(b => b.Partner1)
                        .Include(a => a.SecondCourseGuestTeam1).ThenInclude(b => b.Partner2)
                        .Include(a => a.SecondCourseGuestTeam2).ThenInclude(b => b.Partner1)
                        .Include(a => a.SecondCourseGuestTeam2).ThenInclude(b => b.Partner2)
                        .Include(a => a.ThirdCourseHostTeam).ThenInclude(b => b.Partner1)
                        .Include(a => a.ThirdCourseHostTeam).ThenInclude(b => b.Partner2)
                        .Include(a => a.ThirdCourseGuestTeam1).ThenInclude(b => b.Partner1)
                        .Include(a => a.ThirdCourseGuestTeam1).ThenInclude(b => b.Partner2)
                        .Include(a => a.ThirdCourseGuestTeam2).ThenInclude(b => b.Partner1)
                        .Include(a => a.ThirdCourseGuestTeam2).ThenInclude(b => b.Partner2);
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
                }

                // Remove contact from contact list
                string apiKey = Configuration?.GetEmailSettings("apiKey");
                string apiSecret = Configuration?.GetEmailSettings("apiSecret");
                if (team.Partner1 != null)
                {
                    await EmailSender.RemoveListRecipientAsync(apiKey, apiSecret, team.Partner1.ListRecipientId);
                }

                if (team.Partner2 != null)
                {
                    await EmailSender.RemoveListRecipientAsync(apiKey, apiSecret, team.Partner2.ListRecipientId);
                }

                TeamsRepository.Delete(team);
                TeamsRepository.SaveChanges();
                TempData["StatusMessage"] = "Du hast dein Team vom Event abgemeldet.";
            }

            else
            {
                TempData["StatusMessage"] = "Du hast dich vom Event abgemeldet.";
            }

            return RedirectToPage("RegistrationData", new { id = eventId });
        }
    }
}
