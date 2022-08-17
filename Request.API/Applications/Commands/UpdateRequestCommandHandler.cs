using MediatR;
using Request.Domain.Interfaces;

namespace Request.API.Applications.Commands
{
    public class UpdateRequestCommandHandler : IRequestHandler<UpdateRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateRequestCommandHandler> _logger;

        public UpdateRequestCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<UpdateRequestCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id);
                if (leaveRequest == null) throw new ArgumentNullException();
                leaveRequest.Update(request.DayOffStart,
                    request.DayOffEnd,
                    request.CompensationDayStart,
                    request.CompensationDayEnd,
                    request.Message);
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