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

        private IRequestTypeRepository _requestTypeRepository;

        private IStateRepository _stateRepository;

        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext dbContext,
            IUserRepository userRepository,
            ILeaveRequestRepository leaveRequestRepository,
            IRequestTypeRepository requestTypeRepository,
            IStateRepository stateRepository)
        {
            _dbContext = dbContext;
            _userRepository = userRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _requestTypeRepository = requestTypeRepository;
            _stateRepository = stateRepository;
        }

        public IUserRepository userRepository => _userRepository;

        public ILeaveRequestRepository leaveRequestRepository => _leaveRequestRepository;

        public IRequestTypeRepository requestTypeRepository => _requestTypeRepository;

        public IStateRepository stateRepository => _stateRepository;

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