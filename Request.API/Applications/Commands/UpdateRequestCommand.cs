using MediatR;
using System.Runtime.Serialization;

namespace Request.API.Applications.Commands
{
    [DataContract]
    public class UpdateRequestCommand : IRequest<bool>
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public DateTime DayOffStart { get; set; }

        [DataMember]
        public DateTime DayOffEnd { get; set; }

        [DataMember]
        public DateTime? CompensationDayStart { get; set; }

        [DataMember]
        public DateTime? CompensationDayEnd { get; set; }

        [DataMember]
        public string? Message { get; set; }

        public UpdateRequestCommand()
        {
        }

        public UpdateRequestCommand(DateTime dayOffStart,
            DateTime dayOffEnd,
            DateTime? compensationDayStart,
            DateTime? compensationDayEnd,
            string? message)
        {
            DayOffStart = dayOffStart;
            DayOffEnd = dayOffEnd;
            CompensationDayStart = compensationDayStart;
            CompensationDayEnd = compensationDayEnd;
            Message = message;
        }
    }
}