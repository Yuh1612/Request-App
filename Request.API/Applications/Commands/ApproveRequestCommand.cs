using MediatR;

namespace Request.API.Applications.Commands
{
    public class ApproveRequestCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public string? Description { get; set; }

        public ApproveRequestCommand(Guid id, Guid statusId, string? description)
        {
            Id = id;
            StatusId = statusId;
            Description = description;
        }

        public ApproveRequestCommand()
        {
        }
    }
}