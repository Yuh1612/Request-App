using Request.Domain.Base;
using Request.Domain.Entities.Users;
using System.Diagnostics.CodeAnalysis;

namespace Request.Domain.Entities.Requests
{
    public class Stage : Entity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid? LeaveRequestId { get; set; }
        public virtual LeaveRequest LeaveRequest { get; set; }
    }
}