using MediatR;

namespace Request.API.Applications.Commands
{
    public class ConductRequestCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public Guid StatusId { get; set; }
        public string? Description { get; set; }

        public ConductRequestCommand(Guid id, Guid statusId, string? description)
        {
            Id = id;
            StatusId = statusId;
            Description = description;
        }

        public ConductRequestCommand()
        {
        }
    }
}