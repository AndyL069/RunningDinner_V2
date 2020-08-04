using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Data
{
    public class RoutesRepository : Repository<Route>, IRoutesRepository
    {
        public RoutesRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}

