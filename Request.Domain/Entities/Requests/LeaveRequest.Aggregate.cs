namespace Request.Domain.Entities.Requests
{
    public partial class LeaveRequest
    {
        public LeaveRequest(
            Guid requestorId,
            Guid approverId,
            DateTime dayOffStart,
            DateTime dayOffEnd,
            DateTime? compensationDayStart,
            DateTime? compensationDayEnd,
            Guid statusId,
            string? message)
        {
            this.Id = Guid.NewGuid();
            this.RequestorId = requestorId;
            this.ApproverId = approverId;
            this.DayOffStart = dayOffStart;
            this.DayOffEnd = dayOffEnd;
            this.CompensationDayStart = compensationDayStart;
            this.CompensationDayEnd = compensationDayEnd;
            this.StatusId = statusId;
            this.ApproverId = approverId;
            this.Message = message;

            if (CompensationDayStart == null || CompensationDayEnd == null)
            {
                this.Name = "Nghỉ phép";
            }
            else
            {
                this.Name = "Nghỉ phép và làm bù";
            }
        }

        public void Update(DateTime dayOffStart,
            DateTime dayOffEnd,
            DateTime? compensationDayStart,
            DateTime? compensationDayEnd,
            string? message)
        {
            this.DayOffStart = dayOffStart;
            this.DayOffEnd = dayOffEnd;
            this.CompensationDayStart = compensationDayStart ?? CompensationDayStart;
            this.CompensationDayEnd = compensationDayEnd ?? CompensationDayEnd;
            this.Message = message ?? Message;
        }

        public void AddState(Stage stage)
        {
            if (this.Stages.Any(x => x.Id == stage.Id)) return;
            this.Stages.Add(stage);
        }

        public void RemoveState(Stage stage)
        {
            if (!this.Stages.Any(x => x.Id == stage.Id)) return;
            this.Stages.Remove(stage);
        }

        public void Delete()
        {
            this.IsDelete = true;
        }
    }
}