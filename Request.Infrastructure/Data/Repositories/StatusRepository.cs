using Microsoft.EntityFrameworkCore;
using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Infrastructure.Data.Repositories
{
    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        public StatusRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Status> GetStatusByName(string name)
        {
            return await dbSet.Where(x => x.Name == name).FirstAsync();
        }
    }
}