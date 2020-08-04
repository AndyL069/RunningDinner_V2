using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class MyRouteModel : PageModel
    {
        private IConfiguration Configuration { get; set; }

        private IDinnerEventsRepository DinnerEventsRepository { get; set; }

        private ITeamsRepository TeamsRepository { get; set; }

        private IEventParticipationsRepository EventParticipationsRepository { get; set; }

        private IRoutesRepository RoutesRepository { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;

        public MyRouteModel(UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            IRoutesRepository routesRepository,
            ITeamsRepository teamsRepository,
            IEventParticipationsRepository eventParticipationsRepository,
            IDinnerEventsRepository dinnerEventsRepository)
        {
            _userManager = userManager;
            Configuration = configuration;
            RoutesRepository = routesRepository;
            TeamsRepository = teamsRepository;
            EventParticipationsRepository = eventParticipationsRepository;
            DinnerEventsRepository = dinnerEventsRepository;
        }

        public DinnerEvent Event { get; set; }

        public Team FirstCourseTeam { get; set; }

        public Team SecondCourseTeam { get; set; }

        public Team ThirdCourseTeam { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("EventId") == null)
            {
                string returnUrl = HttpContext.Request.Path.ToString();
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            ViewData["IsParticipating"] = HttpContext.Session.GetString("IsParticipating");
            var eventId = int.Parse(HttpContext.Session.GetString("EventId"), CultureInfo.CurrentCulture);
            ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null)
            {
                return Page();
            }

            bool isParticipating = IsUserParticipating(eventId, user.Id);
            ViewData["IsParticipating"] = isParticipating;
            HttpContext.Session.SetString("IsParticipating", isParticipating.ToString(CultureInfo.CurrentCulture));
            DinnerEvent dinnerEvent = DinnerEventsRepository.GetById(eventId);
            if (dinnerEvent != null)
            {
                // Values are passed-through to layout page
                ViewData["EventName"] = dinnerEvent.EventName;
                ViewData["EventDate"] = dinnerEvent.EventDate.ToShortDateString();
                ViewData["EventCity"] = dinnerEvent.City;
                ViewData["EventPictureLink"] = dinnerEvent.EventPictureLink;
            }

            if (dinnerEvent == null) return Page();
            ViewData["apiKey"] = Configuration?.GetMapsSettings("apiKeyJs");
            ViewData["MapCenterLat"] = "50.113745";
            ViewData["MapCenterLong"] = "8.679317";
            Event = dinnerEvent;

            // Load Team
            Team team = TeamsRepository.SearchFor(x => x.Event.Id == dinnerEvent.Id && (x.Partner1.Id == user.Id || x.Partner2.Id == user.Id)).Include("Partner1").Include("Partner2").Include("Event").FirstOrDefault();

            if (!dinnerEvent.RoutesOpen)
            {
                return Page();
            }

            // Routes
            Route route = RoutesRepository.SearchFor(x => x.Event.Id == eventId && x.RouteForTeam.Id == team.Id)
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
                .Include(a => a.ThirdCourseGuestTeam2).ThenInclude(b => b.Partner2).FirstOrDefault();

            FirstCourseTeam = route.FirstCourseHostTeam;
            SecondCourseTeam = route.SecondCourseHostTeam;
            ThirdCourseTeam = route.ThirdCourseHostTeam;

            List<string> routeAddresses = new List<string>();
            // First Course Host
            routeAddresses.Add(route.FirstCourseHostTeam.FullAddress + ", " + dinnerEvent.City);
            // Second Course Host
            routeAddresses.Add(route.SecondCourseHostTeam.FullAddress + ", " + dinnerEvent.City);
            // Third Course Host
            routeAddresses.Add(route.ThirdCourseHostTeam.FullAddress + ", " + dinnerEvent.City);

            List<string> kitchenCoordinates = new List<string>();
            foreach (var routeAddress in routeAddresses)
            {
                string coordinateString = HereMapsHelper.GetCoordinatesWithAddressString(routeAddress);
                kitchenCoordinates.Add(coordinateString);
            }

            ViewData["KitchenCoordinates"] = kitchenCoordinates.ToArray();

            List<string> routeCoordinates = new List<string>();
            foreach (var routeAddress in routeAddresses)
            {
                string coordinateString = HereMapsHelper.GetCoordinatesString(routeAddress);
                routeCoordinates.Add(coordinateString);
            }

            ViewData["RouteCoordinates"] = routeCoordinates;
            string partyLocationCoordinateString = HereMapsHelper.GetCoordinatesWithAddressString(dinnerEvent.PartyLocation);
            ViewData["PartyLocation"] = partyLocationCoordinateString;
            return Page();
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
    }
}
