using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Data
{
    public class DinnerEventsRepository : Repository<DinnerEvent>, IDinnerEventsRepository
    {
        public DinnerEventsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}


