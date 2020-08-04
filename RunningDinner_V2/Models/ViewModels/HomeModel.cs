using System;
using System.Collections.Generic;

namespace RunningDinner.Models.ViewModels
{
    public class HomeModel
    {
        public List<EventsOverview> CurrentEvents { get; set; }
        public List<EventsOverview> PastEvents { get; set; }
        public string Email { get; set; }
    }

    public class EventsOverview
    {
        public int Id { get; set; }
        public DateTime EventDate { get; set; }

        public DateTime LastRegisterDate { get; set; }

        public string EventName { get; set; }

        public bool RoutesOpen { get; set; }

        public string EventPictureLink { get; set; }

        public bool EventEnabled { get; set; }

        public int ParticipantsCount { get; set; }
    }
}
