using MediatR;
using System.Runtime.Serialization;

namespace Request.API.Applications.Commands
{
    [DataContract]
    public class DeleteRequestCommand : IRequest<bool>
    {
        [DataMember]
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