using MediatR;
using Microsoft.EntityFrameworkCore;
using Request.API.Applications.Queries;
using Request.Domain.Interfaces;
using Request.Domain.Interfaces.Repositories;
using Request.Infrastructure.Data;
using Request.Infrastructure.Data.Repositories;
using System.Reflection;

namespace Request.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            RegisterDbContext(services);

            RegisterRepositories(services);

            RegisterUnitOfWork(services);

            RegisterMediators(services);

            services.AddScoped<IRequestQueries, RequestQueries>();
        }

        private void RegisterDbContext(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetSection("ConnectionString").Value);
            });
        }

        private void RegisterMediators(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        private void RegisterUnitOfWork(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<IStageRepository, StageRepository>();
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }
}