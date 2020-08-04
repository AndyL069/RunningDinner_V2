using System.ComponentModel.DataAnnotations;

namespace RunningDinner.Models.DatabaseModels
{
    public class EventParticipation
    {
        [Key]
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public DinnerEvent Event { get; set; }
        public bool InvitationMailSent { get; set; }
        public string Address { get; set; }
        public string AddressNumber { get; set; }
        public string AddressAdditions { get; set; }
        public string Phone { get; set; }
        public string PartnerName { get; set; }
        public string Allergies { get; set; }
        public string PartnerLastName { get; set; }
        public string PartnerEmail { get; set; }
        public string SelectedCourse { get; set; }
        public bool IsWithoutPartner { get; set; }
        public bool DinnerSaver { get; set; }
        public string SelectedKitchenOption { get; set; }
        public bool RegistrationComplete { get; set; }
    }
}
