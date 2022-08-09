using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Domain.Entities.Requests
{
    public partial class LeaveRequest
    {
        public LeaveRequest(Guid requestTypeId,
            Guid requestorId,
            DateTime dayOffStart, 
            DateTime dayOffEnd, 
            DateTime compensationDayStart, 
            DateTime compensationDayEnd)
        {
            this.RequestorId = requestorId;
            this.RequestTypeId = requestTypeId;
            this.DayOffStart = dayOffStart;
            this.DayOffEnd = dayOffEnd;
            this.CompensationDayStart = compensationDayStart;
            this.CompensationDayEnd = compensationDayEnd;
        }

        public void AddState(State state)
        {
            if (this.States.Any(x => x.Id == state.Id)) return;
            this.States.Add(state);
        }

        public void RemoveState(State state)
        {
            if (!this.States.Any(x => x.Id == state.Id)) return;
            this.States.Remove(state);
        }

    }
}
