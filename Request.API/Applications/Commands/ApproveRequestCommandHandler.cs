using API.Exceptions;
using MediatR;
using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class ApproveRequestCommandHandler : IRequestHandler<ApproveRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ApproveRequestCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ApproveRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<ApproveRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(ApproveRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handle command: {command}", nameof(ApproveRequestCommandHandler));

                await _unitOfWork.BeginTransaction();

                if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == "id").Value,
                    out var approverId))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindApprovedAsync(request.Id, approverId);
                if (leaveRequest == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Request not found");

                var status = await _unitOfWork.statusRepository.FindAsync(request.StatusId);
                if (status == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Status not found");

                leaveRequest.UpdateStatus(status.Id);
                leaveRequest.AddStage(StageEnum.Finish, request.Description);
                _unitOfWork.leaveRequestRepository.Update(leaveRequest);

                return await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                _logger.LogError("Cannot handle command: {command}", nameof(ApproveRequestCommandHandler));
                throw;
            }
        }
    }
}