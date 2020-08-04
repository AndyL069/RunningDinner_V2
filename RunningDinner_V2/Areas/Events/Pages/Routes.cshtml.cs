using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using GeoCoordinatePortable;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RunningDinner.Data;
using RunningDinner.Helpers;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Events.Pages
{
    [Authorize(Roles = "Administrator")]
    public class RoutesModel : PageModel
    {
        private IDinnerEventsRepository DinnerEventsRepository { get; set; }

        private IEventParticipationsRepository EventParticipationsRepository { get; set; }

        private ITeamsRepository TeamsRepository { get; set; }

        private IRoutesRepository RoutesRepository { get; set; }

        public RoutesModel(IDinnerEventsRepository dinnerEventsRepository,
            IEventParticipationsRepository eventParticipationsRepository,
            IRoutesRepository routesRepository,
            ITeamsRepository teamsRepository)
        {
            DinnerEventsRepository = dinnerEventsRepository;
            EventParticipationsRepository = eventParticipationsRepository;
            RoutesRepository = routesRepository;
            TeamsRepository = teamsRepository;
        }

        public List<SelectListItem> Courses { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Participants { get; set; } = new List<SelectListItem>();

        public List<SelectListItem> Addresses { get; set; } = new List<SelectListItem>();

        [BindProperty]
        public List<SelectListItem> Teams { get; set; }

        [BindProperty]
        public List<Route> Routes { get; set; }

        public List<SelectListItem> Users { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("EventId") == null)
            {
                string returnUrl = HttpContext.Request.Path.ToString();
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            ViewData["IsParticipating"] = HttpContext.Session.GetString("IsParticipating");
            var eventId = int.Parse(HttpContext.Session.GetString("EventId"), CultureInfo.CurrentCulture);
            DinnerEvent dinnerEvent = DinnerEventsRepository.GetById(eventId);
            if (dinnerEvent != null)
            {
                // Values are passed-through to layout page
                ViewData["EventId"] = dinnerEvent.Id;
                ViewData["EventName"] = dinnerEvent.EventName;
                ViewData["EventDate"] = dinnerEvent.EventDate.ToShortDateString();
                ViewData["EventCity"] = dinnerEvent.City;
                ViewData["EventPictureLink"] = dinnerEvent.EventPictureLink;
            }

            List<EventParticipation> participants = EventParticipationsRepository.SearchFor(x => x.Event.Id == eventId).Include("User").OrderBy(p => p.User.FirstName).ToList();
            List<SelectListItem> participantsSelectList = new List<SelectListItem> { new SelectListItem { Value = "empty", Text = "" } };
            foreach (EventParticipation participant in participants)
            {
                participantsSelectList.Add(new SelectListItem { Value = participant.User.Id, Text = participant.User.FirstName + " " + participant.User.LastName });
            }

            var teams = TeamsRepository.SearchFor(x => x.Event.Id == eventId);
            List<string> addresses = teams.OrderBy(e => e.AddressStreet).Select(e => e.AddressStreet + " " + e.AddressNumber).ToList();
            List<SelectListItem> addressesSelectList = new List<SelectListItem> { new SelectListItem { Value = "empty", Text = "" } };
            foreach (string address in addresses)
            {
                addressesSelectList.Add(new SelectListItem { Value = address, Text = address });
            }

            List<Route> routes;
            using (var context = new ApplicationDbContext())
            {
                routes = context.Routes.AsNoTracking().Where(er => er.Event.Id == eventId).Include("Event")
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
                .Include(a => a.ThirdCourseGuestTeam2).ThenInclude(b => b.Partner2)
                .ToList();
            }

            Users = participantsSelectList;
            Addresses = addressesSelectList;
            Participants = participantsSelectList;
            Courses = new List<SelectListItem>
            {
                new SelectListItem {Value = "FirstCourse", Text = "Vorspeise"},
                new SelectListItem {Value = "SecondCourse", Text = "Hauptgang"},
                new SelectListItem {Value = "ThirdCourse", Text = "Nachtisch"}
            };

            var teamsList = TeamsRepository.SearchFor(x => x.Event.Id == eventId).ToList();
            List<SelectListItem> teamsSelectList = new List<SelectListItem> { new SelectListItem { Value = "empty", Text = "" } };
            foreach (var team in teamsList)
            {
                teamsSelectList.Add(new SelectListItem { Value = team.Id.ToString(), Text = "Team " + team.Id.ToString() });
            }

            Teams = teamsSelectList;
            Routes = routes;
            return Page();
        }

        /// <summary>
        /// PageHandler for creating routes automatically.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPostCreateRoutes(int factor)
        {
            if (HttpContext.Session.GetString("EventId") == null)
            {
                string returnUrl = HttpContext.Request.Path.ToString();
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            ViewData["IsParticipating"] = HttpContext.Session.GetString("IsParticipating");
            var eventId = int.Parse(HttpContext.Session.GetString("EventId"), CultureInfo.CurrentCulture);
            var dinnerEvent = DinnerEventsRepository.GetById(eventId);
            List<CalculationHost> FirstCourseHosts = new List<CalculationHost>();
            List<CalculationHost> SecondCourseHosts = new List<CalculationHost>();
            List<CalculationHost> ThirdCourseHosts = new List<CalculationHost>();
            var teams = TeamsRepository.SearchFor(x => x.Event.Id == eventId).ToList();
            int routeFactor = factor;
            int numberOfRoutes = routeFactor * 3;
            // First Course Teams
            foreach(Team team in teams)
            {
                if (team.SelectedCourse == "FirstCourse")
                {
                    FirstCourseHosts.Add(new CalculationHost() { HostTeam = team });
                }

                if (team.SelectedCourse == "SecondCourse")
                {
                    SecondCourseHosts.Add(new CalculationHost() { HostTeam = team });
                }

                if (team.SelectedCourse == "ThirdCourse")
                {
                    ThirdCourseHosts.Add(new CalculationHost() { HostTeam = team });
                }
            }

            List<CalculationHost> FirstCourseHostsOrdered = new List<CalculationHost>();
            List<CalculationHost> SecondCourseHostsOrdered = SecondCourseHosts;
            List<CalculationHost> ThirdCourseHostsOrdered = new List<CalculationHost>();
            //////////////////////////////// Get GeoCoordinates
            foreach (var secondCourseHost in SecondCourseHosts)
            {
                var address = secondCourseHost.HostTeam.AddressStreet + " " + secondCourseHost.HostTeam.AddressNumber + "," + secondCourseHost.HostTeam.City;
                secondCourseHost.Coordinates = HereMapsHelper.GetCoordinates(address);
            }

            foreach (var thirdCourseHost in ThirdCourseHosts)
            {
                var address = thirdCourseHost.HostTeam.AddressStreet + " " + thirdCourseHost.HostTeam.AddressNumber + "," + thirdCourseHost.HostTeam.City;
                thirdCourseHost.Coordinates = HereMapsHelper.GetCoordinates(address);
            }

            foreach (var firstCourseHost in FirstCourseHosts)
            {
                var address = firstCourseHost.HostTeam.AddressStreet + " " + firstCourseHost.HostTeam.AddressNumber + "," + firstCourseHost.HostTeam.City;
                firstCourseHost.Coordinates = HereMapsHelper.GetCoordinates(address);
            }

            /////////////////////////////// Calculate Distances
            foreach (var secondCourseHost in SecondCourseHosts)
            {
                // From Second to First course hosts
                secondCourseHost.HostDistancesToFirst = new List<RouteNodeDistance>();
                foreach (var firstCourseHost in FirstCourseHosts)
                {
                    RouteNodeDistance hostDistance = new RouteNodeDistance();
                    hostDistance.Host1 = secondCourseHost;
                    hostDistance.Distance = secondCourseHost.Coordinates.GetDistanceTo(firstCourseHost.Coordinates);
                    hostDistance.Host2 = firstCourseHost;
                    secondCourseHost.HostDistancesToFirst.Add(hostDistance);
                }

                secondCourseHost.HostDistancesToFirst = secondCourseHost.HostDistancesToFirst.OrderBy(x => x.Distance).ToList();
                // From Second to Third course hosts
                secondCourseHost.HostDistancesToThird = new List<RouteNodeDistance>();
                foreach (var thirdCourseHost in ThirdCourseHosts)
                {
                    RouteNodeDistance hostDistance = new RouteNodeDistance();
                    hostDistance.Host1 = secondCourseHost;
                    hostDistance.Distance = secondCourseHost.Coordinates.GetDistanceTo(thirdCourseHost.Coordinates);
                    hostDistance.Host2 = thirdCourseHost;
                    secondCourseHost.HostDistancesToThird.Add(hostDistance);
                }

                secondCourseHost.HostDistancesToThird = secondCourseHost.HostDistancesToThird.OrderBy(x => x.Distance).ToList();
            }

            //////////////////////// Re-order hosts by distance 
            foreach (var secondCourseHost in SecondCourseHosts)
            {
                // Shortest Distance from SecondCourse to FirstCourse
                foreach (var hostDistance in secondCourseHost.HostDistancesToFirst)
                {
                    if (!FirstCourseHostsOrdered.Contains(hostDistance.Host2))
                    {
                        FirstCourseHostsOrdered.Add(hostDistance.Host2);
                        break;
                    }
                }
            }

            int k = 0;
            foreach (var secondCourseHost in SecondCourseHosts)
            {
                // Start from second SecondCourseHost
                if (k == 0)
                {
                    k++;
                    continue;
                }

                // Shortest Distance from SecondCourse To ThirdCourse 
                foreach (var hostDistance in secondCourseHost.HostDistancesToThird)
                {
                    if (!ThirdCourseHostsOrdered.Contains(hostDistance.Host2))
                    {
                        ThirdCourseHostsOrdered.Add(hostDistance.Host2);
                        break;
                    }
                }

                k++;
            }

            // Handle remaining SecondCourseHost
            var firstSecondCourseHost = SecondCourseHostsOrdered.FirstOrDefault();
            // SecondCourse To ThirdCourse 
            foreach (var hostDistance in firstSecondCourseHost.HostDistancesToThird)
            {
                if (!ThirdCourseHostsOrdered.Contains(hostDistance.Host2))
                {
                    ThirdCourseHostsOrdered.Add(hostDistance.Host2);
                    break;
                }
            }

            //////////////////// Set SecondCourseHosts (use only Ordered Hosts from here!)
            // Start from First
            int i = 0;
            // Start from Second
            int j = 1;
            foreach (var secondCourseHost in SecondCourseHostsOrdered)
            {
                // Add FirstCourseHost
                FirstCourseHostsOrdered[i].GuestTeam1 = secondCourseHost.HostTeam;
                // Add ThirdCourseHost
                ThirdCourseHostsOrdered[j].GuestTeam1 = secondCourseHost.HostTeam;
                i++;
                j++;
                if (i == routeFactor)
                {
                    i = 0;
                }

                if (j == routeFactor)
                {
                    j = 0;
                }
            }

            /////////////////////////////////// Set ThirdCourseHosts
            // Start from First
            i = 0;
            // Start from Second
            j = 1;
            foreach (var thirdCourseHost in ThirdCourseHostsOrdered)
            {
                // Add FirstCourseHost
                FirstCourseHostsOrdered[i].GuestTeam2 = thirdCourseHost.HostTeam;
                // Add SecondCourseHost         
                SecondCourseHostsOrdered[j].GuestTeam1 = thirdCourseHost.HostTeam;
                i++;
                j++;
                if (i == routeFactor)
                {
                    i = 0;
                }

                if (j == routeFactor)
                {
                    j = 0;
                }
            }

            /////////////////////////////////// Set FirstCourseHosts
            // Start from Third
            i = 2;
            // Start from Third
            j = 2;
            foreach (var firstCourseHost in FirstCourseHostsOrdered)
            {
                // Add SecondCourseHost
                SecondCourseHostsOrdered[i].GuestTeam2 = firstCourseHost.HostTeam;
                // Add ThirdCourseHost              
                ThirdCourseHostsOrdered[j].GuestTeam2 = firstCourseHost.HostTeam;
                i++;
                j++;
                if (i == routeFactor)
                {
                    i = 0;
                }

                if (j == routeFactor)
                {
                    j = 0;
                }
            }

            ///////////////////////// Create routes
            // Add SecondCourse routes
            foreach (var host in SecondCourseHostsOrdered)
            {
                Route newRoute = new Route();
                newRoute.Event = dinnerEvent;
                newRoute.RouteForTeam = host.HostTeam;
                // Second Course
                newRoute.SecondCourseHostTeam = host.HostTeam;
                newRoute.SecondCourseGuestTeam1 = host.GuestTeam1;
                newRoute.SecondCourseGuestTeam2 = host.GuestTeam2;
                // First Course
                var firstCourseHostList = from r in FirstCourseHostsOrdered
                                      where r.GuestTeam1.Id == host.HostTeam.Id
                                      select r;
                var firstCourseHost = firstCourseHostList.FirstOrDefault();
                newRoute.FirstCourseHostTeam = firstCourseHost.HostTeam;
                newRoute.FirstCourseGuestTeam1 = firstCourseHost.GuestTeam1;
                newRoute.FirstCourseGuestTeam2 = firstCourseHost.GuestTeam2;
                // Third Course
                var thirdCourseHostList = from r in ThirdCourseHostsOrdered
                                      where r.GuestTeam1.Id == host.HostTeam.Id
                                      select r;
                var thirdCourseHost = thirdCourseHostList.FirstOrDefault();
                newRoute.ThirdCourseHostTeam = thirdCourseHost.HostTeam;
                newRoute.ThirdCourseGuestTeam1 = thirdCourseHost.GuestTeam1;
                newRoute.ThirdCourseGuestTeam2 = thirdCourseHost.GuestTeam2;
                RoutesRepository.Insert(newRoute);
            }

            // Add ThirdCourse routes
            foreach (var host in ThirdCourseHostsOrdered)
            {
                Route newRoute = new Route();
                newRoute.Event = dinnerEvent;
                newRoute.RouteForTeam = host.HostTeam;
                // Third Course
                newRoute.ThirdCourseHostTeam = host.HostTeam;
                newRoute.ThirdCourseGuestTeam1 = host.GuestTeam1;
                newRoute.ThirdCourseGuestTeam2 = host.GuestTeam2;
                // First Course
                var firstCourseHostList = from r in FirstCourseHostsOrdered
                                      where r.GuestTeam2.Id == host.HostTeam.Id
                                      select r;
                var firstCourseHost = firstCourseHostList.FirstOrDefault();
                newRoute.FirstCourseHostTeam = firstCourseHost.HostTeam;
                newRoute.FirstCourseGuestTeam1 = firstCourseHost.GuestTeam1;
                newRoute.FirstCourseGuestTeam2 = firstCourseHost.GuestTeam2;
                // Second Course
                var secondCourseHostList = from r in SecondCourseHostsOrdered
                                       where r.GuestTeam1.Id == host.HostTeam.Id
                                       select r;
                var secondCourseHost = secondCourseHostList.FirstOrDefault();
                newRoute.SecondCourseHostTeam = secondCourseHost.HostTeam;
                newRoute.SecondCourseGuestTeam1 = secondCourseHost.GuestTeam1;
                newRoute.SecondCourseGuestTeam2 = secondCourseHost.GuestTeam2;
                RoutesRepository.Insert(newRoute);
            }

            // Add FirstCourse routes
            foreach (var host in FirstCourseHostsOrdered)
            {
                Route newRoute = new Route();
                newRoute.Event = dinnerEvent;
                newRoute.RouteForTeam = host.HostTeam;
                // First Course
                newRoute.FirstCourseHostTeam = host.HostTeam;
                newRoute.FirstCourseGuestTeam1 = host.GuestTeam1;
                newRoute.FirstCourseGuestTeam2 = host.GuestTeam2;
                // Second Course
                var secondCourseHostList = from r in SecondCourseHostsOrdered
                                       where r.GuestTeam2.Id == host.HostTeam.Id
                                       select r;
                var secondCourseHost = secondCourseHostList.FirstOrDefault();
                newRoute.SecondCourseHostTeam = secondCourseHost.HostTeam;
                newRoute.SecondCourseGuestTeam1 = secondCourseHost.GuestTeam1;
                newRoute.SecondCourseGuestTeam2 = secondCourseHost.GuestTeam2;
                // Third Course
                var thirdCourseHostList = from r in ThirdCourseHostsOrdered
                                      where r.GuestTeam2.Id == host.HostTeam.Id
                                      select r;
                var thirdCourseHost = thirdCourseHostList.FirstOrDefault();
                newRoute.ThirdCourseHostTeam = thirdCourseHost.HostTeam;
                newRoute.ThirdCourseGuestTeam1 = thirdCourseHost.GuestTeam1;
                newRoute.ThirdCourseGuestTeam2 = thirdCourseHost.GuestTeam2;
                RoutesRepository.Insert(newRoute);
            }

            RoutesRepository.SaveChanges();
            return RedirectToPage();
        }

        /// <summary>
        /// Saves a route.
        /// </summary>
        /// <returns></returns>
        public IActionResult OnPost(Route route)
        {
            if (HttpContext.Session.GetString("EventId") == null)
            {
                string returnUrl = HttpContext.Request.Path.ToString();
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            ViewData["IsParticipating"] = HttpContext.Session.GetString("IsParticipating");
            if (route != null)
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingRoute = context.Routes.FirstOrDefault(x => x.Id == route.Id);
                    context.Entry(existingRoute).State = EntityState.Detached;
                    existingRoute.RouteForTeam = route.RouteForTeam;
                    existingRoute.FirstCourseHostTeam = route.FirstCourseHostTeam;
                    existingRoute.FirstCourseGuestTeam1 = route.FirstCourseGuestTeam1;
                    existingRoute.FirstCourseGuestTeam2 = route.FirstCourseGuestTeam2;
                    existingRoute.SecondCourseHostTeam = route.SecondCourseHostTeam;
                    existingRoute.SecondCourseGuestTeam1 = route.SecondCourseGuestTeam1;
                    existingRoute.SecondCourseGuestTeam2 = route.SecondCourseGuestTeam2;
                    existingRoute.ThirdCourseHostTeam = route.ThirdCourseHostTeam;
                    existingRoute.ThirdCourseGuestTeam1 = route.ThirdCourseGuestTeam1;
                    existingRoute.ThirdCourseGuestTeam2 = route.ThirdCourseGuestTeam2;
                    context.Entry(existingRoute).State = EntityState.Modified;
                    context.SaveChanges();
                }
            }

            return RedirectToPage();
        }

        /// <summary>
        /// Removes a route.
        /// </summary>
        /// <param name="routeId"></param>
        public IActionResult OnGetRemoveRoute(int id)
        {
            var route = RoutesRepository.GetById(id);
            RoutesRepository.Delete(route);
            RoutesRepository.SaveChanges();
            return OnGet();
        }

        /// <summary>
        /// Removes all routes.
        /// </summary>
        /// <param name="routeId"></param>
        public IActionResult OnGetRemoveAllRoutes(int eventId)
        {
            var routes = RoutesRepository.SearchFor(x => x.Event.Id == eventId);
            foreach (var route in routes)
            {
                RoutesRepository.Delete(route);
            }         

            RoutesRepository.SaveChanges();
            return OnGet();
        }

        /// <summary>
        /// Adds a route.
        /// </summary>
        /// <param name="routeId"></param>
        public IActionResult OnGetAddRoute(int eventId)
        {
            var route = new Route();
            var dinnerEvent = DinnerEventsRepository.GetById(eventId);
            route.Event = dinnerEvent;
            RoutesRepository.Insert(route);
            RoutesRepository.SaveChanges();
            return OnGet();
        }

        public class RouteNodeDistance
        {
            public CalculationHost Host1 { get; set; }
            public CalculationHost Host2 { get; set; }
            public double Distance { get; set; }
        }

        public class CalculationHost
        {
            public Team HostTeam { get; set; }
            public Team GuestTeam1 { get; set; }
            public Team GuestTeam2 { get; set; }
            public GeoCoordinate Coordinates { get; set; }
            public List<RouteNodeDistance> HostDistancesToFirst { get; set; }
            public List<RouteNodeDistance> HostDistancesToThird { get; set; }
        }

        public class CalculationRoute
        {
            public int Id { get; set; }
            public CalculationHost FirstCourse { get; set; }
            public CalculationHost SecondCourse { get; set; }
            public CalculationHost ThirdCourse { get; set; }
        }

    }
}

