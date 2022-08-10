using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces.Repositories;

namespace Request.Infrastructure.Data.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}