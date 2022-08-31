using API.Exceptions;
using MediatR;
using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class CancelRequestCommandHandler : BaseCommandHandler, IRequestHandler<CancelRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CancelRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<CancelRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CancelRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handle command: {command}", nameof(CancelRequestCommandHandler));

                await _unitOfWork.BeginTransaction();

                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id, GetCurrentUserId());
                if (leaveRequest == null)
                {
                    _logger.LogError("Request not found");
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }


                var status = await _unitOfWork.statusRepository.GetStatusByName(StatusEnum.Cancel);
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
                _logger.LogError("Cannot handle command: {command}", nameof(CancelRequestCommandHandler));
                throw;
            }
        }
    }
}