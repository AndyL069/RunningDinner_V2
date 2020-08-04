using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<MailLog> MailLogs { get; set; }
        public DbSet<DinnerEvent> DinnerEvents { get; set; }
        public DbSet<EventParticipation> EventParticipations { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code.
            //See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlServer(Startup.ConnectionString);
        }
    }
}
