using MediatR;
using Request.Domain.Interfaces;
using Request.Domain.Interfaces.Repositories;

namespace Request.API.Applications.Commands
{
    public class UpdateRequestCommandHandler 
        : IRequestHandler<UpdateRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;
        private readonly ILogger<UpdateRequestCommandHandler> _logger;
        public UpdateRequestCommandHandler(IMediator mediator,
            IUnitOfWork unitOfWork,
            ILogger<UpdateRequestCommandHandler> logger
            )
        {

        }
        public async Task<bool> Handle(UpdateRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                await _unitOfWork.leaveRequestRepository.InsertAsync(
                    new Domain.Entities.Requests.LeaveRequest(request.RequestorId,
                    request.DayOffStart,
                    request.DayOffEnd,
                    request.CompensationDayStart,
                    request.CompensationDayEnd));
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
            
        }
    }
}
