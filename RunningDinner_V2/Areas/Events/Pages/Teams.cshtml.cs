using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RunningDinner.Data;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Areas.Events.Pages
{
    [Authorize(Roles = "Administrator")]
    public class TeamsModel : PageModel
    {
        private IDinnerEventsRepository DinnerEventsRepository { get; set; }

        private ITeamsRepository TeamsRepository { get; set; }

        public TeamsModel(IDinnerEventsRepository dinnerEventsRepository,
            ITeamsRepository teamsRepository)
        {
            DinnerEventsRepository = dinnerEventsRepository;
            TeamsRepository = teamsRepository;
        }

        public List<Team> Teams { get; set; }

        public IActionResult OnGet()
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

            List<Team> teams = TeamsRepository.SearchFor(x => x.Event.Id == eventId).Include("Partner1").Include("Partner2").ToList();
            Teams = teams;
            return Page();
        }

    }
}


