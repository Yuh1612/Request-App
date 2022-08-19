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

        public ConductRequestCommandHandler(IUnitOfWork unitOfWork, ILogger<ConductRequestCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(ConductRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Handle command: {command}", nameof(ConductRequestCommandHandler));

                await _unitOfWork.BeginTransaction();
                var leaveRequest = await _unitOfWork.leaveRequestRepository.FindAsync(request.Id);
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