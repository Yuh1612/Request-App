using Request.Domain.Base;

namespace Request.Domain.Entities.Requests
{
    public class Stage : Entity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime? CreateAt { get; set; }

        public Guid? LeaveRequestId { get; set; }
        public virtual LeaveRequest LeaveRequest { get; set; }
    }
}