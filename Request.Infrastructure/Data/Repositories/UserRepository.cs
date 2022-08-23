using Request.Domain.Entities.Users;
using Request.Domain.Interfaces.Repositories;

namespace Request.Infrastructure.Data.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}