using MediatR;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Request.API.Applications.Commands
{
    [DataContract]
    public class CreateRequestCommand : IRequest<bool>
    {
        [DataMember]
        public Guid RequestorId { get; set; }

        [DataMember]
        public Guid ApproverId { get; set; }

        [DataMember]
        public DateTime DayOffStart { get; set; }

        [DataMember]
        public DateTime DayOffEnd { get; set; }

        [DataMember]
        public DateTime? CompensationDayStart { get; set; }

        [DataMember]
        public DateTime? CompensationDayEnd { get; set; }

        [DataMember]
        public Guid StatusId { get; set; }

        [DataMember]
        public string? Message { get; set; }

        public CreateRequestCommand()
        {
        }

        public CreateRequestCommand(
            Guid requesttorId,
            Guid approverId,
            DateTime dayOffStart,
            DateTime dayOffEnd,
            DateTime compensationDayStart,
            DateTime compensationDayEnd,
            Guid statusId,
            string message)
        {
            RequestorId = requesttorId;
            ApproverId = approverId;
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