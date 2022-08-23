using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces.Repositories;

namespace Request.Infrastructure.Data.Repositories
{
    public class StageRepository : GenericRepository<Stage>, IStageRepository
    {
        public StageRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}