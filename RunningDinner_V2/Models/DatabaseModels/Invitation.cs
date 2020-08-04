using System;
using System.ComponentModel.DataAnnotations;

namespace RunningDinner.Models.DatabaseModels
{
    public class Invitation
    {
        [Key]
        public int Id { get; set; }

        public DinnerEvent Event { get; set; }

        public ApplicationUser User { get; set; }

        public string InvitationEmail { get; set; }

        public DateTime SentTime { get; set; }

        public bool InvitationAccepted { get; set; }
    }
}