using MediatR;

namespace Request.API.Applications.Commands
{
    public class CancelRequestCommand : IRequest<bool>
    {
        public CancelRequestCommand(Guid id, string? description)
        {
            Id = id;
            Description = description;
        }

        public CancelRequestCommand()
        {
        }

        public Guid Id { get; set; }
        public string? Description { get; set; }
    }
}