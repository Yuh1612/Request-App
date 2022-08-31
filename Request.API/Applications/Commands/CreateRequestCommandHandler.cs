using API.Exceptions;
using MediatR;
using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class CreateRequestCommandHandler : BaseCommandHandler, IRequestHandler<CreateRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransaction();

                var leaveRequest = new LeaveRequest(GetCurrentUserId(),
                        request.ApproverId,
                        request.DayOffStart,
                        request.DayOffEnd,
                        request.CompensationDayStart,
                        request.CompensationDayEnd,
                        request.Message);

                leaveRequest.StatusId = StatusEnum.Waiting;
                leaveRequest.AddStage(StageEnum.Process, null);
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