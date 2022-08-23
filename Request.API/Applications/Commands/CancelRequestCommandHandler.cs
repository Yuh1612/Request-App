using API.Exceptions;
using MediatR;
using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class CancelRequestCommandHandler : IRequestHandler<CancelRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CancelRequestCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CancelRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<CancelRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(CancelRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handle command: {command}", nameof(CancelRequestCommandHandler));

                await _unitOfWork.BeginTransaction();

                if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == "id").Value,
                    out var userId))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id, userId);
                if (leaveRequest == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Request not found");

                var status = await _unitOfWork.statusRepository.GetStatusByName(StatusEnum.Cancel);
                if (status == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Status not found");

                leaveRequest.UpdateStatus(status.Id);
                leaveRequest.AddStage(StageEnum.Finish, request.Description);
                _unitOfWork.leaveRequestRepository.Update(leaveRequest);

                return await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                _logger.LogError("Cannot handle command: {command}", nameof(CancelRequestCommandHandler));
                throw;
            }
        }
    }
}