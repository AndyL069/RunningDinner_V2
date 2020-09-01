using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RunningDinner.Data;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Events.Pages
{
    public class ParticipantsModel : PageModel
    {
        private IDinnerEventsRepository DinnerEventsRepository { get; set; }

        private readonly UserManager<ApplicationUser> _userManager;

        private IEventParticipationsRepository EventParticipationsRepository { get; set; }

        public ParticipantsModel(IDinnerEventsRepository dinnerEventsRepository,
            IEventParticipationsRepository eventParticipationsRepository,
            UserManager<ApplicationUser> userManager)
        {
            DinnerEventsRepository = dinnerEventsRepository;
            EventParticipationsRepository = eventParticipationsRepository;
            _userManager = userManager;
        }

        public List<ApplicationUser> Participants { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("EventId") == null)
            {
                string returnUrl = HttpContext.Request.Path.ToString();
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            ViewData["IsParticipating"] = HttpContext.Session.GetString("IsParticipating");
            int eventId = int.Parse(HttpContext.Session.GetString("EventId"), CultureInfo.CurrentCulture);
            ViewData["EventId"] = eventId;
            DinnerEvent dinnerEvent = DinnerEventsRepository.GetById(eventId);
            if (dinnerEvent != null)
            {
                // Values are passed-through to layout page
                ViewData["EventName"] = dinnerEvent.EventName;
                ViewData["EventDate"] = dinnerEvent.EventDate.ToShortDateString();
                ViewData["EventCity"] = dinnerEvent.City;
                ViewData["EventPictureLink"] = dinnerEvent.EventPictureLink;
            }

            List<EventParticipation> participations = EventParticipationsRepository.SearchFor(x => x.Event.Id == eventId).Include("User").ToList();
            List<ApplicationUser> participants = new List<ApplicationUser>();
            foreach (EventParticipation participation in participations)
            {
                var user = await _userManager.FindByIdAsync(participation.User.Id);
                participants.Add(user);
            }

            Participants = participants;
            return Page();
        }
    }
}
