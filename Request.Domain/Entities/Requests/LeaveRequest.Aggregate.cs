using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Domain.Entities.Requests
{
    public partial class LeaveRequest
    {
        public LeaveRequest(
            Guid requestorId,
            DateTime dayOffStart, 
            DateTime dayOffEnd, 
            DateTime compensationDayStart, 
            DateTime compensationDayEnd)
        {
            this.RequestorId = requestorId;
            this.DayOffStart = dayOffStart;
            this.DayOffEnd = dayOffEnd;
            this.CompensationDayStart = compensationDayStart;
            this.CompensationDayEnd = compensationDayEnd;
        }

        public void AddState(Stage state)
        {
            if (this.States.Any(x => x.Id == state.Id)) return;
            this.States.Add(state);
        }

        public void RemoveState(Stage state)
        {
            if (!this.States.Any(x => x.Id == state.Id)) return;
            this.States.Remove(state);
        }

    }
}
