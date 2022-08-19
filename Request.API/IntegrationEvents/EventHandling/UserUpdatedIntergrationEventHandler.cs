using EventBus.Abstractions;
using Request.API.IntegrationEvents.Events;
using Request.Domain.Entities.Users;
using Request.Domain.Interfaces;

namespace Request.API.IntegrationEvents.EventHandling
{
    public class UserUpdatedIntergrationEventHandler : IIntegrationEventHandler<UserUpdatedIntergrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<UserUpdatedIntergrationEventHandler> _logger;

        public UserUpdatedIntergrationEventHandler(IUnitOfWork unitOfWork, ILogger<UserUpdatedIntergrationEventHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(UserUpdatedIntergrationEvent @event)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var user = await _unitOfWork.userRepository.FindAsync(@event.Id);
                if (user == null) throw new ArgumentNullException("User is null");
                user.Update(@event.UserName);
                _logger.LogInformation("Update new user");
                _unitOfWork.userRepository.Update(user);
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