using RunningDinner.Models.DatabaseModels;

namespace RunningDinner.Data
{
    public class InvitationsRepository : Repository<Invitation>, IInvitationsRepository
    {
        public InvitationsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
    }
}


