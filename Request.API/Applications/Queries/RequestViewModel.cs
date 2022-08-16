namespace Request.API.Applications.Queries
{
    public record LeaveRequestReponse
    {
        public Guid RequestorId { get; set; }
        public string RequestorName { get; set; }
        public Guid StatusId { get; set; }
        public string StatusName { get; set; }
        public Guid? Approver { get; set; }
        public string LeaveRequestName { get; set; }
    }
}
