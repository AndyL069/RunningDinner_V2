using Microsoft.AspNetCore.Identity;

namespace RunningDinner.Models.DatabaseModels
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public bool SendEventNewsletter { get; set; }
        public string ProfilePicture { get; set; }
        public string TemporaryProfilePicture { get; set; }
        public string ContactData { get; set; }
        public int ContactId { get; set; }
        public long ListRecipientId { get; set; }
    }
}
