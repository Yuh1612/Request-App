using EventBus.Abstractions;
using Request.API.IntegrationEvents.Events;
using Request.Domain.Entities.Users;
using Request.Domain.Interfaces;

namespace Request.API.IntegrationEvents.EventHandling
{
    public class UserCreatedIntergrationEventHandler : IIntegrationEventHandler<UserCreatedIntergrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserCreatedIntergrationEventHandler> _logger;

        public UserCreatedIntergrationEventHandler(ILogger<UserCreatedIntergrationEventHandler> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UserCreatedIntergrationEvent @event)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var user = new User(@event.Id, @event.UserName, @event.Email);
                await _unitOfWork.userRepository.InsertAsync(user);
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransaction();
                _logger.LogError(ex.Message);
            }
        }
    }
}