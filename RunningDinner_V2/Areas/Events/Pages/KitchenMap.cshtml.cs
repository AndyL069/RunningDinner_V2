using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Authorize(Roles = "Administrator")]
    public class KitchenMapModel : PageModel
    {
        private IDinnerEventsRepository DinnerEventsRepository { get; set; }



        private IRoutesRepository RoutesRepository { get; set; }

        private IConfiguration Configuration { get; set; }

        public KitchenMapModel(IDinnerEventsRepository dinnerEventsRepository,
            IRoutesRepository routesRepository,
            IConfiguration configuration)
        {
            DinnerEventsRepository = dinnerEventsRepository;
            RoutesRepository = routesRepository;
            Configuration = configuration;
        }

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
                ViewData["EventName"] = dinnerEvent.EventName;
                ViewData["EventDate"] = dinnerEvent.EventDate.ToShortDateString();
                ViewData["EventCity"] = dinnerEvent.City;
                ViewData["EventPictureLink"] = dinnerEvent.EventPictureLink;
            }

            else
            {
                return RedirectToPage("RegistrationData");
            }

            ViewData["apiKey"] = Configuration?.GetMapsSettings("apiKeyJs");
            ViewData["MapCenterLat"] = "50.113745";
            ViewData["MapCenterLong"] = "8.679317";          

            // Routes
            List<Route> routes = RoutesRepository.SearchFor(x => x.Event.Id == eventId)
                .Include(a => a.Event)
                .Include(a => a.RouteForTeam)
                .Include(a => a.FirstCourseHostTeam)
                .Include(a => a.FirstCourseGuestTeam1)
                .Include(a => a.FirstCourseGuestTeam2)
                .Include(a => a.SecondCourseHostTeam)
                .Include(a => a.SecondCourseGuestTeam1)
                .Include(a => a.SecondCourseGuestTeam2)
                .Include(a => a.ThirdCourseHostTeam)
                .Include(a => a.ThirdCourseGuestTeam1)
                .Include(a => a.ThirdCourseGuestTeam2).ToList();
            List<string> kitchenAddresses = new List<string>();
            foreach (var route in routes)
            {
                if (route.RouteForTeam?.Id == route.FirstCourseHostTeam?.Id)
                {
                    kitchenAddresses.Add(route.FirstCourseHostTeam.FullAddress + "," + dinnerEvent.City);
                }

                if (route.RouteForTeam?.Id == route.SecondCourseHostTeam?.Id)
                {
                    kitchenAddresses.Add(route.SecondCourseHostTeam.FullAddress + "," + dinnerEvent.City);
                }

                if (route.RouteForTeam?.Id == route.ThirdCourseHostTeam?.Id)
                {
                    kitchenAddresses.Add(route.ThirdCourseHostTeam.FullAddress + "," + dinnerEvent.City);
                }
            }

            List<string> kitchenCoordinates = new List<string>();
            foreach(var kitchenAddress in kitchenAddresses)
            {
                string coordinateString = HereMapsHelper.GetCoordinatesWithAddressString(kitchenAddress);
                kitchenCoordinates.Add(coordinateString);
            }
            
            ViewData["KitchenCoordinates"] = kitchenCoordinates.ToArray();
            List<string> routeAddresses = new List<string>();
            foreach (Route route in routes)
            {
                // First Course Host
                routeAddresses.Add(route.FirstCourseHostTeam.FullAddress + ", " + dinnerEvent.City);
                // Second Course Host
                routeAddresses.Add(route.SecondCourseHostTeam.FullAddress + ", " + dinnerEvent.City);
                // Third Course Host
                routeAddresses.Add(route.ThirdCourseHostTeam.FullAddress + ", " + dinnerEvent.City);
            }

            List<string> routeCoordinates = new List<string>();
            foreach (var routeAddress in routeAddresses)
            {
                string coordinateString = HereMapsHelper.GetCoordinatesString(routeAddress);
                routeCoordinates.Add(coordinateString);
            }

            ViewData["RouteCoordinates"] = routeCoordinates.ToArray();
            string partyLocationCoordinateString = HereMapsHelper.GetCoordinatesWithAddressString(dinnerEvent.PartyLocation);
            ViewData["PartyLocation"] = partyLocationCoordinateString;
            return Page();
        }
    }
}
