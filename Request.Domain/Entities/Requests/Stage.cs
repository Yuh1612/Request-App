using Request.Domain.Base;

namespace Request.Domain.Entities.Requests
{
    public class Stage : Entity
    {
        public Stage()
        {

        }
        public Stage(string name, string? description, Guid? leaveRequestId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            CreateAt = DateTime.Now;
            LeaveRequestId = leaveRequestId;
        }

        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateAt { get; set; }
        public Guid? LeaveRequestId { get; set; }
        public virtual LeaveRequest LeaveRequest { get; set; }
    }
}