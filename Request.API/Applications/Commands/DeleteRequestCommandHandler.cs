using API.Exceptions;
using MediatR;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class DeleteRequestCommandHandler : BaseCommandHandler, IRequestHandler<DeleteRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id, GetCurrentUserId());
                if (leaveRequest == null)
                {
                    _logger.LogError("Request not found");
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                leaveRequest.Delete();
                _unitOfWork.leaveRequestRepository.Update(leaveRequest);
                return await _unitOfWork.SaveChangeAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                throw;
            }
        }
    }
}