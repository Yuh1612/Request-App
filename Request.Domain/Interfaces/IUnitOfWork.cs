using Request.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; }

        ILeaveRequestRepository leaveRequestRepository { get; }
        IStageRepository stageRepository { get;  }

        Task SaveChangeAsync();

        Task BeginTransaction();

        Task CommitTransaction();

        Task RollbackTransaction();
    }
}
