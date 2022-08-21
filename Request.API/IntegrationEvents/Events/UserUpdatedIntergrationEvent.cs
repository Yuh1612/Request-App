using EventBus.Events;

namespace Request.API.IntegrationEvents.Events
{
    public class UserUpdatedIntergrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public UserUpdatedIntergrationEvent(Guid id, string userName)
        {
            Id = id;
            UserName = userName;
        }
    }
}