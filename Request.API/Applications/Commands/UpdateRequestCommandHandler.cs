using API.Exceptions;
using MediatR;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class UpdateRequestCommandHandler : BaseCommandHandler, IRequestHandler<UpdateRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor, logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id, GetCurrentUserId());
                if (leaveRequest == null)
                {
                    _logger.LogError("Request not found");
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                leaveRequest.Update(request.DayOffStart,
                    request.DayOffEnd,
                    request.CompensationDayStart,
                    request.CompensationDayEnd,
                    request.Message);

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