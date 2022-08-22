using Request.Domain.Entities.Requests;

namespace Request.Domain.Interfaces.Repositories
{
    public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
    {
        public Task<LeaveRequest?> FindAsync(Guid Id, Guid userId);
    }
}