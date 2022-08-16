using MediatR;
using System.Runtime.Serialization;

namespace Request.API.Applications.Commands
{
    [DataContract]
    public class UpdateRequestCommand : IRequest<bool>
    {
        
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
        public UpdateRequestCommand()
        {

        }
        public UpdateRequestCommand(Guid requesttorId,
            DateTime dayOffStart,
            DateTime dayOffEnd,
            DateTime compensationDayStart,
            DateTime compensationDayEnd)
        {
            RequestorId = requesttorId;
            DayOffStart = dayOffStart;
            DayOffEnd = dayOffEnd;
            CompensationDayStart = compensationDayStart;
            CompensationDayEnd = compensationDayEnd;
        }
    }
}
