using Request.Domain.Interfaces.Repositories;

namespace Request.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }
        ILeaveRequestRepository leaveRequestRepository { get; }
        IStageRepository stageRepository { get; }

        IStatusRepository statusRepository { get; }

        Task<bool> SaveChangeAsync();

        Task BeginTransaction();

        Task<bool> CommitTransaction();

        Task RollbackTransaction();
    }
}