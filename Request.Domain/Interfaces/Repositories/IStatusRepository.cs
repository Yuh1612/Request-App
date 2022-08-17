using Request.Domain.Entities.Requests;

namespace Request.Domain.Interfaces.Repositories
{
    public interface IStatusRepository : IGenericRepository<Status>
    {
        public Task<Status> GetStatusByName(string name);
    }
}