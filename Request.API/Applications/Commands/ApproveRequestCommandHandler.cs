using API.Exceptions;
using MediatR;
using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class ApproveRequestCommandHandler : BaseCommandHandler, IRequestHandler<ApproveRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApproveRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<ApproveRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(ApproveRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handle command: {command}", nameof(ApproveRequestCommandHandler));

                await _unitOfWork.BeginTransaction();

                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindApprovedAsync(request.Id, GetCurrentUserId());
                if (leaveRequest == null)
                {
                    _logger.LogError("Request not found");
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                var status = await _unitOfWork.statusRepository.FindAsync(request.StatusId);
                if (status == null)
                {

                    _logger.LogError("Status not found");
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                }

                leaveRequest.StatusId = status.Id;
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