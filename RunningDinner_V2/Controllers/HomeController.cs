using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RunningDinner.Models;
using RunningDinner.Data;
using RunningDinner.Models.ViewModels;
using System.Collections.Generic;
using RunningDinner.Models.DatabaseModels;
using System.Linq;
using System;

namespace RunningDinner.Controllers
{
    public class HomeController : Controller
    {
        IDinnerEventsRepository DinnerEventsRepository { get; set; }
        IEventParticipationsRepository EventParticipationsRepository { get; set; }

        public HomeController(
            IDinnerEventsRepository dinnerEventsRepository,
            IEventParticipationsRepository eventParticipationsRepository)
        {
            DinnerEventsRepository = dinnerEventsRepository;
            EventParticipationsRepository = eventParticipationsRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<DinnerEvent>events = DinnerEventsRepository.GetAll().ToList();
            var currentEvents = events.Where(x => x.EventDate >= DateTime.Today).ToList();
            var pastEvents = events.Where(x => x.EventDate < DateTime.Today).ToList();
            List<EventsOverview> CurrentEventsList = new List<EventsOverview>();
            foreach (var currentEvent in currentEvents)
            {
                CurrentEventsList.Add(new EventsOverview
                {
                    Id = currentEvent.Id,
                    EventDate = currentEvent.EventDate,
                    EventEnabled = currentEvent.EventEnabled,
                    EventName = currentEvent.EventName,
                    EventPictureLink = currentEvent.EventPictureLink,
                    LastRegisterDate = currentEvent.LastRegisterDate,
                    RoutesOpen = currentEvent.RoutesOpen,
                    ParticipantsCount = GetParticipantsNumber(currentEvent.Id)
                });
            };

            List<EventsOverview> PastEventsList = new List<EventsOverview>();
            foreach (var pastEvent in pastEvents)
            {
                PastEventsList.Add(new EventsOverview
                {
                    Id = pastEvent.Id,
                    EventDate = pastEvent.EventDate,
                    EventEnabled = pastEvent.EventEnabled,
                    EventName = pastEvent.EventName,
                    EventPictureLink = pastEvent.EventPictureLink,
                    LastRegisterDate = pastEvent.LastRegisterDate,
                    RoutesOpen = pastEvent.RoutesOpen,
                    ParticipantsCount = GetParticipantsNumber(pastEvent.Id)
                });
            };

            HomeModel viewModel = new HomeModel
            {
                CurrentEvents = CurrentEventsList,
                PastEvents = PastEventsList
            };

            return View(viewModel);
        }

        [HttpGet]
        public int GetParticipantsNumber(int eventId)
        {
            var dinnerEvent = EventParticipationsRepository.SearchFor(x => x.Event.Id == eventId).ToList();
            return dinnerEvent.Count;
        }

        [HttpPost]
        public IActionResult Index(HomeModel viewModel)
        {
            return View(viewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
