using API.Exceptions;
using MediatR;
using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class CreateRequestCommandHandler : IRequestHandler<CreateRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateRequestCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CreateRequestCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<UpdateRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Requestor is null!");
                throw new HttpResponseException(HttpStatusCode.NotFound, "Request is null!");
            }

            if (request.ApproverId == Guid.Empty)
            {
                _logger.LogWarning("Approver is null!");
                throw new HttpResponseException(HttpStatusCode.NotFound, "Approver is null!");
            }

            try
            {
                await _unitOfWork.BeginTransaction();

                if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == "id").Value,
                    out var requestorId))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var leaveRequest = new LeaveRequest(requestorId,
                        request.ApproverId,
                        request.DayOffStart,
                        request.DayOffEnd,
                        request.CompensationDayStart,
                        request.CompensationDayEnd,
                        request.Message);
                var status = await _unitOfWork.statusRepository.GetStatusByName(StatusEnum.Waiting);
                leaveRequest.UpdateStatus(status.Id);
                leaveRequest.AddStage(StageEnum.Process, "Chờ Minh Trí Lê");
                await _unitOfWork.leaveRequestRepository.InsertAsync(leaveRequest);
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