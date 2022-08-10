using Request.Domain.Base;
using Request.Domain.Entities.Users;

namespace Request.Domain.Entities.Requests
{
    public partial class LeaveRequest : Entity
    {
        public LeaveRequest()
        {
            this.Stages = new HashSet<Stage>();
        }

        public Guid RequestorId { get; set; }
        public virtual User Requestor { get; set; }
        public Guid StatusId { get; set; }
        public virtual Status Status { get; set; }
        public DateTime DayOffStart { get; set; }
        public DateTime DayOffEnd { get; set; }
        public DateTime CompensationDayStart { get; set; }
        public DateTime CompensationDayEnd { get; set; }
        public virtual ICollection<Stage> Stages { get; set; }
    }
}