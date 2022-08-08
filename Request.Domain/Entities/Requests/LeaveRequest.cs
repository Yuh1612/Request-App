using Request.Domain.Base;
using Request.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace Request.Domain.Entities.Requests
{
    public partial class LeaveRequest : Entity
    {
        public LeaveRequest()
        {
            this.States = new HashSet<State>();
        }
        public virtual RequestType RequestType { get; set; }
        public virtual User Requestor { get; set; }
        public DateTime DayOffStart { get; set; }
        public DateTime DayOffEnd { get; set; }
        public DateTime CompensationDayStart { get; set; }
        public DateTime CompensationDayEnd { get; set; }
        public virtual ICollection<State> States { get; set; }


    }
}
