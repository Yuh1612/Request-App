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
        [AllowNull]
        public string Name { get; set; }
        [AllowNull]
        public Guid? RequestorId { get; set; }
        public virtual User? Requestor { get; set; }
        [AllowNull]
        public Guid StatusId { get; set; }
        public virtual Status Status { get; set; }
        [AllowNull]
        public DateTime DayOffStart { get; set; }
        [AllowNull]
        public DateTime DayOffEnd { get; set; }
        [AllowNull]
        public DateTime CompensationDayStart { get; set; }
        [AllowNull]
        public DateTime CompensationDayEnd { get; set; }
        [AllowNull]
        public string Message { get; set; }
        [AllowNull]
        public Guid? ApproverId { get; set; }
        public virtual User? Approver { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
        [AllowNull]
        public DateTime CreatedAt { get; set; }
        [AllowNull]
        public DateTime UpdatedAt { get; set; }
    }
}