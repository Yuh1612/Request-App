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