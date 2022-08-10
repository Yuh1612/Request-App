using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Request.Domain.Interfaces;
using Request.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbContextTransaction? _transaction;

        private IsolationLevel? _isolationLevel;

        private IUserRepository _userRepository;

        private ILeaveRequestRepository _leaveRequestRepository;

        private IStageRepository _stageRepository;

        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext,
            IUserRepository userRepository,
            ILeaveRequestRepository leaveRequestRepository,
            IStageRepository stageRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _stageRepository = stageRepository;
        }

        public IUserRepository userRepository => _userRepository;

        public ILeaveRequestRepository leaveRequestRepository => _leaveRequestRepository;

        public IStageRepository stageRepository => _stageRepository;

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

        public async Task CommitTransaction()
        {
            await _dbContext.SaveEntitiesAsync();

            if (_transaction == null) return;
            await _transaction.CommitAsync();

            await _transaction.DisposeAsync();
            _transaction = null;
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

        public async Task SaveChangeAsync()
        {
            await _dbContext.SaveEntitiesAsync();
        }
    }
}