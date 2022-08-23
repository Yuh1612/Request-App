using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Request.Domain.Interfaces;
using Request.Domain.Interfaces.Repositories;
using System.Data;

namespace Request.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        private IsolationLevel? _isolationLevel;

        private IUserRepository _userRepository;

        private ILeaveRequestRepository _leaveRequestRepository;

        private IStageRepository _stageRepository;

        private IStatusRepository _statusRepository;

        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext,
            IUserRepository userRepository,
            ILeaveRequestRepository leaveRequestRepository,
            IStageRepository stageRepository,
            IStatusRepository statusRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _stageRepository = stageRepository;
            _statusRepository = statusRepository;
        }

        public IUserRepository userRepository => _userRepository;

        public ILeaveRequestRepository leaveRequestRepository => _leaveRequestRepository;

        public IStageRepository stageRepository => _stageRepository;

        public IStatusRepository statusRepository => _statusRepository;

        public async Task BeginTransaction()
        {
            if (_transaction == null)
            {
                if (_isolationLevel.HasValue)
                {
                    _transaction = await _dbContext.Database.BeginTransactionAsync(_isolationLevel.Value);
                }
                else
                {
                    _transaction = await _dbContext.Database.BeginTransactionAsync();
                }
            }
        }

        public async Task<bool> CommitTransaction()
        {
            var result = await _dbContext.SaveEntitiesAsync();

            if (_transaction == null) return false;
            await _transaction.CommitAsync();

            await _transaction.DisposeAsync();
            _transaction = null;

            return result;
        }

        public void Dispose()
        {
            if (_dbContext == null) return;

            _dbContext.Dispose();
        }

        public async Task RollbackTransaction()
        {
            if (_transaction == null) return;

            await _transaction.RollbackAsync();

            await _transaction.DisposeAsync();
            _transaction = null;
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _dbContext.SaveEntitiesAsync();
        }
    }
}