using MediatR;
using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces;

namespace Request.API.Applications.Commands
{
    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateRequestCommandHandler> _logger;

        public CreateRequestCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<UpdateRequestCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                await _unitOfWork.leaveRequestRepository.InsertAsync(

                    new LeaveRequest(request.RequestorId,
                        request.ApproverId,
                        request.DayOffStart,
                        request.DayOffEnd,
                        request.CompensationDayStart,
                        request.CompensationDayEnd,
                        request.StatusId,
                        request.ApproverId,
                        request.Message));
                return await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                return false;
            }
        }
    }
}