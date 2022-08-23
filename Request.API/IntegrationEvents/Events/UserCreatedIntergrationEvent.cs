using EventBus.Events;

namespace Request.API.IntegrationEvents.Events
{
    public class UserCreatedIntergrationEvent : IntegrationEvent
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public UserCreatedIntergrationEvent(Guid id, string username, string email)
        {
            Id = id;
            UserName = username;
            Email = email;
        }
    }
}