using MediatR;
using Microsoft.EntityFrameworkCore;
using Request.Domain.Entities.Requests;
using Request.Domain.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IMediator _mediator;

        public ApplicationDbContext(IMediator mediator)
        {
            _mediator = mediator;
        }
        public DbSet<User> Users { get; set; }

        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<RequestType> RequestTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"server=192.168.2.231;database=Intern_RequestModule;user id=sa;password=vStation123;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<LeaveRequest>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<RequestType>().HasQueryFilter(p => !p.IsDelete);
            modelBuilder.Entity<State>().HasQueryFilter(p => !p.IsDelete);
        }

        public async Task SaveEntitiesAsync()
        {
            await SaveChangesAsync();
            await _mediator.DispatchDomainEventsAsync(this);
        }
    }
}
