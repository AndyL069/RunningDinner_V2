using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Data
{
    public class EventParticipationsRepository : Repository<EventParticipation>, IEventParticipationsRepository
    {
        public EventParticipationsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
