using EventBus.Abstractions;
using EventBus;
using EventBusRabbitMQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Request.API.Applications.Queries;
using Request.API.IntegrationEvents.EventHandling;
using Request.API.IntegrationEvents.Events;
using Request.Domain.Interfaces;
using Request.Domain.Interfaces.Repositories;
using Request.Infrastructure.Data;
using Request.Infrastructure.Data.Repositories;
using System.Reflection;
using RabbitMQ.Client;
using Request.API.Applications.Commands;
using API.Middleware;
using API.Exceptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
            services.AddControllers(options =>
            {
                options.Filters.Add<HttpResponseExceptionFilter>();
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddHttpContextAccessor();

            RegisterDbContext(services);

            RegisterRepositories(services);

            RegisterUnitOfWork(services);

            RegisterMediators(services);

            RegisterEventBus(services);

            RegisterRabbitMQ(services);

            RegisterAuth(services);

            services.AddScoped<IRequestQueries, RequestQueries>();
        }

        private void RegisterAuth(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("SecretKey").Value)),

                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        private void RegisterRabbitMQ(IServiceCollection services)
        {
            services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var rabbitMQ = Configuration.GetSection("RabbitMQ");
                var factory = new ConnectionFactory()
                {
                    HostName = rabbitMQ.GetSection("Hostname").Value,
                    DispatchConsumersAsync = true,
                    UserName = rabbitMQ.GetSection("Username").Value,
                    Password = rabbitMQ.GetSection("Password").Value,
                };

                var retryCount = Int32.Parse(Configuration.GetSection("EventBusRetryCount").Value);

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMQServices>(sp =>
            {
                var subscriptionClientName = "queue_test";
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQServices>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var serviceScopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
                var retryCount = 5;

                return new EventBusRabbitMQServices(rabbitMQPersistentConnection, logger, eventBusSubcriptionsManager, serviceScopeFactory, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<IIntegrationEventHandler<UserCreatedIntergrationEvent>, UserCreatedIntergrationEventHandler>();
            services.AddTransient<IIntegrationEventHandler<UserUpdatedIntergrationEvent>, UserUpdatedIntergrationEventHandler>();
        }

        private void RegisterDbContext(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseLazyLoadingProxies()
                    .UseSqlServer(Configuration.GetSection("ConnectionString").Value);
            });
        }

        private void RegisterMediators(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(UpdateRequestCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(CreateRequestCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(DeleteRequestCommand).GetTypeInfo().Assembly);
            services.AddMediatR(typeof(ConductRequestCommand).GetTypeInfo().Assembly);
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
            services.AddScoped<IStatusRepository, StatusRepository>();
        }

        public void Configure(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            ConfigureEventBus(app);

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseMiddleware<RequestLoggerMiddleware>();

            app.UseAuthorization();

            app.MapControllers();
        }

        private void ConfigureEventBus(WebApplication app)
        {
            var eventBus = app.Services.GetRequiredService<IEventBus>();

            eventBus.Subscribe<UserCreatedIntergrationEvent, IIntegrationEventHandler<UserCreatedIntergrationEvent>>();
            eventBus.Subscribe<UserUpdatedIntergrationEvent, IIntegrationEventHandler<UserUpdatedIntergrationEvent>>();
        }
    }
}