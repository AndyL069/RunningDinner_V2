using System.ComponentModel.DataAnnotations;

namespace RunningDinner.Models.DatabaseModels
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        public DinnerEvent Event { get; set; }

        public ApplicationUser Partner1 { get; set; }

        public ApplicationUser Partner2 { get; set; }

        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        [Display(Name = "Straße")]
        public string AddressStreet { get; set; }

        [Display(Name = "Hausnummer")]
        public string AddressNumber { get; set; }

        public string FullAddress { get; set; }

        [Display(Name = "Adresszusatz")]
        public string AddressAdditions { get; set; }

        public string City { get; set; }

        public bool DinnerSaver { get; set; }

        public string SelectedCourse { get; set; }

        public string Allergies { get; set; }

    }
}
