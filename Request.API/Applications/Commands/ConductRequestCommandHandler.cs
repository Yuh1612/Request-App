using API.Exceptions;
using MediatR;
using Request.Domain.Entities.Requests;
using Request.Domain.Interfaces;
using System.Net;

namespace Request.API.Applications.Commands
{
    public class ConductRequestCommandHandler : IRequestHandler<ConductRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ConductRequestCommandHandler> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ConductRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<ConductRequestCommandHandler> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Handle(ConductRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handle command: {command}", nameof(ConductRequestCommandHandler));

                await _unitOfWork.BeginTransaction();

                if (Guid.TryParse(_httpContextAccessor.HttpContext.User.Claims.First(i => i.Type == "id").Value,
                    out var requestorId))
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id, requestorId);
                if (leaveRequest == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Request not found");

                var status = await _unitOfWork.statusRepository.FindAsync(request.StatusId);
                if (status == null) throw new HttpResponseException(HttpStatusCode.NotFound, "Status not found");

                leaveRequest.UpdateStatus(status.Id);
                leaveRequest.AddStage(StageEnum.Finish, "Trí minh lê kết thúc");
                _unitOfWork.leaveRequestRepository.Update(leaveRequest);

                return await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransaction();
                _logger.LogError("Cannot handle command: {command}", nameof(ConductRequestCommandHandler));
                throw;
            }
        }
    }
}