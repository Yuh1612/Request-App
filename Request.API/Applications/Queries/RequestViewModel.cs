using Request.Domain.Entities.Requests;

namespace Request.API.Applications.Queries
{
    public record LeaveRequestResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StageName { get; set; }
        public Guid StatusId { get; set; }
        public string StatusName { get; set; }
        public Guid? ApproverId { get; set; }
        public string ApproverName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public record LeaveRequestDetail
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public Guid? RequestorId { get; set; }
        public string RequestorName { get; set; }
        public Guid StatusId { get; set; }
        public string StatusName { get; set; }
        public Guid? ApproverId { get; set; }
        public string ApproverName { get; set; }
        public List<StageResponse> Stages { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
    public record StageResponse
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateAt { get; set; }
    }

    public record LeaveRequestDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string StageName { get; set; }
        public string StatusName { get; set; }
        public string ApproverName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}