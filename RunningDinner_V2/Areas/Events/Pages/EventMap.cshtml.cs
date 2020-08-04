using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RunningDinner.Data;
using RunningDinner.Extensions;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Events.Pages
{
    public class EventMapModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private IConfiguration Configuration { get; set; }

        private IDinnerEventsRepository DinnerEventsRepository { get; set; }

        private IEventParticipationsRepository EventParticipationsRepository { get; set; }

        public EventMapModel(UserManager<ApplicationUser> userManager,
            IDinnerEventsRepository dinnerEventsRepository,
            IEventParticipationsRepository eventParticipationsRepository,
            IConfiguration configuration)
        {
            _userManager = userManager;
            DinnerEventsRepository = dinnerEventsRepository;
            EventParticipationsRepository = eventParticipationsRepository;
            Configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync(int eventId)
        {
            // Get eventId
            if (eventId == 0)
            {
                if (HttpContext.Session.GetString("EventId") == null)
                {
                    string returnUrl = HttpContext.Request.Path.ToString();
                    return RedirectToPage("/Account/Login", new { area = "Identity" });
                }

                eventId = int.Parse(HttpContext.Session.GetString("EventId"), CultureInfo.CurrentCulture);
            }

            else
            {
                HttpContext.Session.SetString("EventId", eventId.ToString(CultureInfo.CurrentCulture));
            }

            ApplicationUser user = await _userManager.GetUserAsync(User).ConfigureAwait(false);
            if (user == null)
            {
                return Page();
            }

            bool isParticipating = IsUserParticipating(eventId, user.Id);
            ViewData["IsParticipating"] = isParticipating;
            HttpContext.Session.SetString("IsParticipating", isParticipating.ToString(CultureInfo.CurrentCulture));
            DinnerEvent dinnerEvent = DinnerEventsRepository.GetById(eventId);
            ViewData["apiKey"] = Configuration?.GetMapsSettings("apiKeyJs");
            ViewData["MapCenterLat"] = "50.113745";
            ViewData["MapCenterLong"] = "8.679317";
            ViewData["EventName"] = dinnerEvent.EventName;
            ViewData["EventDate"] = dinnerEvent.EventDate.ToShortDateString();
            ViewData["EventCity"] = dinnerEvent.City;
            ViewData["EventPictureLink"] = dinnerEvent.EventPictureLink;
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
