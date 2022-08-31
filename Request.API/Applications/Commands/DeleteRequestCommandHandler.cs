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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DeleteRequestCommandHandler(IUnitOfWork unitOfWork,
            ILogger<DeleteRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == "id").Value,
                    out var requestorId))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id, requestorId);
                if (leaveRequest == null)
                {
                    _logger.LogError("leaveRequest is null!");
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                leaveRequest.Delete();
                _unitOfWork.leaveRequestRepository.Update(leaveRequest);
                return await _unitOfWork.CommitTransaction();
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackTransaction();
                _logger.LogError(e.Message);
                throw new HttpResponseException(HttpStatusCode.BadRequest, "Something went wrong!");
            }
        }
    }
}