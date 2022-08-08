using Request.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
