using System;
using System.ComponentModel.DataAnnotations;

namespace RunningDinner.Models.DatabaseModels
{
    public class DinnerEvent
    {
        [Key]
        public int Id { get; set; }
        public DateTime EventDate { get; set; }

        public DateTime LastRegisterDate { get; set; }

        public string EventName { get; set; }
        [Display(Name = "Stadt")]
        public string City { get; set; }

        public bool RoutesOpen { get; set; }

        public string PartyLocation { get; set; }

        public string PartyLocationName { get; set; }

        public string EventPictureLink { get; set; }

        public bool EventEnabled { get; set; }

        public int ContactList { get; set; }
    }
}