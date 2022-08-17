using MediatR;
using Request.Domain.Interfaces;

namespace Request.API.Applications.Commands
{
    public class DeleteRequestCommandHandler : IRequestHandler<DeleteRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteRequestCommandHandler> _logger;

        public DeleteRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteRequestCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id);
                if (leaveRequest == null) throw new ArgumentNullException();
                leaveRequest.Delete();
                _unitOfWork.leaveRequestRepository.Update(leaveRequest);
                return await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                _logger.LogError("Cannot delete this request");
                return false;
            }
        }
    }
}