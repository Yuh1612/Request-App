namespace Request.Domain.Entities.Requests
{
    public partial class LeaveRequest
    {
        public LeaveRequest(
            Guid requestorId,
            DateTime dayOffStart,
            DateTime dayOffEnd,
            DateTime compensationDayStart,
            DateTime compensationDayEnd,
            Guid statusId,
            string message)
        {
            this.Id = Guid.NewGuid();
            this.RequestorId = requestorId;
            this.DayOffStart = dayOffStart;
            this.DayOffEnd = dayOffEnd;
            this.CompensationDayStart = compensationDayStart;
            this.CompensationDayEnd = compensationDayEnd;
            this.StatusId = statusId;
            this.Message = message;
        }

        public void Update(DateTime dayOffStart,
            DateTime dayOffEnd,
            DateTime compensationDayStart,
            DateTime compensationDayEnd,
            string message)
        {
            this.DayOffStart = dayOffStart;
            this.DayOffEnd = dayOffEnd;
            this.CompensationDayStart = compensationDayStart;
            this.CompensationDayEnd = compensationDayEnd;
            this.Message = message;
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
    }
}