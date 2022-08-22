using API.Exceptions;
using MediatR;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class UpdateRequestCommandHandler : IRequestHandler<UpdateRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateRequestCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateRequestCommandHandler(
            IUnitOfWork unitOfWork,
            ILogger<UpdateRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(UpdateRequestCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogWarning("Request is null!");
                throw new HttpResponseException(HttpStatusCode.NotFound, "Request is null!");
            }
            if (request.DayOffStart == null)
            {
                _logger.LogWarning("Day-off start is null!");
                throw new HttpResponseException(HttpStatusCode.NotFound, "Day-off start is null!");
            }
            if (request.DayOffEnd == null)
            {
                _logger.LogWarning("Day-off end is null!");
                throw new HttpResponseException(HttpStatusCode.NotFound, "Day-off end is null!");
            }

            try
            {
                await _unitOfWork.BeginTransaction();

                if (!Guid.TryParse(_httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == "id").Value,
                    out var requestorId))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id, requestorId);
                if (leaveRequest == null) throw new HttpResponseException(HttpStatusCode.NotFound);

                leaveRequest.Update(request.DayOffStart,
                    request.DayOffEnd,
                    request.CompensationDayStart,
                    request.CompensationDayEnd,
                    request.Message);
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