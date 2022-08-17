using MediatR;
using System.Runtime.Serialization;

namespace Request.API.Applications.Commands
{
    [DataContract]
    public class CreateRequestCommand : IRequest<bool>
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid RequestorId { get; set; }
        [DataMember]
        public DateTime DayOffStart { get; set; }
        [DataMember]
        public DateTime DayOffEnd { get; set; }
        [DataMember]
        public DateTime CompensationDayStart { get; set; }
        [DataMember]
        public DateTime CompensationDayEnd { get; set; }
        [DataMember]
        public Guid StatusId { get; set; }
        [DataMember]
        public Guid ApproverId { get; set; }
        [DataMember]
        public string Message { get; set; }
        public CreateRequestCommand()
        {

        }
        public CreateRequestCommand(
            string name,
            Guid requesttorId,
            DateTime dayOffStart,
            DateTime dayOffEnd,
            DateTime compensationDayStart,
            DateTime compensationDayEnd,
            Guid statusId,
            Guid approverId,
            string message)
        {
            Name = name;
            RequestorId = requesttorId;
            DayOffStart = dayOffStart;
            DayOffEnd = dayOffEnd;
            CompensationDayStart = compensationDayStart;
            CompensationDayEnd = compensationDayEnd;
            StatusId = statusId;
            ApproverId = approverId;
            Message = message;
        }
    }
}
