using Request.Domain.Interfaces.Repositories;

namespace Request.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }

        ILeaveRequestRepository leaveRequestRepository { get; }
        IStageRepository stageRepository { get; }

        Task SaveChangeAsync();

        Task BeginTransaction();

        Task CommitTransaction();

        Task RollbackTransaction();
    }
}