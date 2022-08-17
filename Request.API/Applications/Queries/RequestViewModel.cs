﻿namespace Request.API.Applications.Queries
{
    public record LeaveRequestReponse
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public string StatusName { get; set; }
        public Guid? ApproverId { get; set; }
        public string Name { get; set; }
        public string ApproverName { get; set; }
        public string StageName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
