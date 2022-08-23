using Microsoft.EntityFrameworkCore;
using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces.Repositories;

namespace Request.Infrastructure.Data.Repositories
{
    public class LeaveRequestRepository : GenericRepository<LeaveRequest>, ILeaveRequestRepository
    {
        public LeaveRequestRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<LeaveRequest?> FindApprovedAsync(Guid Id, Guid userId)
        {
            return await dbSet.Where(r => r.Id == Id && r.ApproverId == userId).FirstOrDefaultAsync();
        }

        public async Task<LeaveRequest?> FindAsync(Guid Id, Guid userId)
        {
            return await dbSet.Where(r => r.Id == Id && r.RequestorId == userId).FirstOrDefaultAsync();
        }
    }
}