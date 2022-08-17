using API.Exceptions;
using MediatR;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class DeleteRequestCommandHandler : IRequestHandler<DeleteRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteRequestCommandHandler> _logger;

        public DeleteRequestCommandHandler(IUnitOfWork unitOfWork,
            ILogger<DeleteRequestCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Request is null!");
                throw new HttpResponseException(HttpStatusCode.NotFound, "Request is null!");
            }
            if (request.Id == Guid.Empty)
            {
                _logger.LogWarning("Id is null!");
                throw new HttpResponseException(HttpStatusCode.NotFound, "Id is null!");
            }

            try
            {
                await _unitOfWork.BeginTransaction();
                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id);
                if (leaveRequest == null) throw new ArgumentNullException("Request is not existed!");
                leaveRequest.Delete();
                _unitOfWork.leaveRequestRepository.Update(leaveRequest);
                return await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                _logger.LogError(e.Message);
                throw new HttpResponseException(HttpStatusCode.BadRequest,  "Something went wrong!");
            }
        }
    }
}