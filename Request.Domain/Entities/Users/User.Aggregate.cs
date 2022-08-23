using Request.Domain.Base;

namespace Request.Domain.Entities.Users
{
    public partial class User : IAggregateRoot
    {
        public User(Guid id, string userName, string? email)
        {
            Id = id;
            UserName = userName;
            Email = email;
        }
        public void Update(string userName)
        {
            UserName = userName ?? UserName; 
        }
    }
}