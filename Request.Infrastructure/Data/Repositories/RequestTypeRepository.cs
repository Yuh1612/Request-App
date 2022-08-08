using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Infrastructure.Data.Repositories
{
    public class RequestTypeRepository : GenericRepository<RequestType>, IRequestTypeRepository
    {
        public RequestTypeRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }
    }
}
