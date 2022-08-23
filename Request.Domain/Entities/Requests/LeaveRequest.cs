using Request.Domain.Base;
using Request.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Request.Domain.Entities.Requests
{
    public partial class LeaveRequest : Entity
    {
        public LeaveRequest()
        {
            this.Stages = new HashSet<Stage>();
        }

        public string Name { get; set; }
        public Guid? RequestorId { get; set; }
        public virtual User? Requestor { get; set; }

        public Guid StatusId { get; set; }
        public virtual Status Status { get; set; }

        public DateTime DayOffStart { get; set; }

        public DateTime DayOffEnd { get; set; }

        public DateTime? CompensationDayStart { get; set; }

        public DateTime? CompensationDayEnd { get; set; }

        public string? Message { get; set; }

        public Guid? ApproverId { get; set; }
        public virtual User? Approver { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}