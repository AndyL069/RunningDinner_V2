using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Data
{
    public class TeamsRepository : Repository<Team>, ITeamsRepository
    {
        public TeamsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}

