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
            string? message)
        {
            this.Id = new Guid();
            this.RequestorId = requestorId;
            this.ApproverId = approverId;
            this.DayOffStart = dayOffStart;
            this.DayOffEnd = dayOffEnd;
            this.CompensationDayStart = compensationDayStart;
            this.CompensationDayEnd = compensationDayEnd;
            this.Message = message;

            if (CompensationDayStart == null || CompensationDayEnd == null)
            {
                this.Name = NameEnum.Leave;
            }
            else
            {
                this.Name = NameEnum.LeaveAndCompensate;
            }
            this.Stages = new HashSet<Stage>();
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

        public void UpdateStatus(Guid statusId)
        {
            this.StatusId = statusId;
        }

        public void AddStage(Stage stage)
        {
            this.Stages.Add(stage);
        }

        public void AddStage(string name, string? description)
        {
            var stage = new Stage(name, description, this.Id);
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