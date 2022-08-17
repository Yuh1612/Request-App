using MediatR;
using System.Runtime.Serialization;

namespace Request.API.Applications.Commands
{
    public class DeleteRequestCommand : IRequest<bool>
    {
        public Guid Id { get; set; }

        public DeleteRequestCommand(Guid id)
        {
            Id = id;
        }

        public DeleteRequestCommand()
        {
        }
    }
}