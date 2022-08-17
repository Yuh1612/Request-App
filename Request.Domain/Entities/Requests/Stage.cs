using Request.Domain.Base;
using Request.Domain.Entities.Users;
using System.Diagnostics.CodeAnalysis;

namespace Request.Domain.Entities.Requests
{
    public class Stage : Entity
    {
        [AllowNull]
        public string Name { get; set; }
        public Guid? LeaveRequestId { get; set; }
        public virtual LeaveRequest LeaveRequest { get; set; }
        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
    }
}