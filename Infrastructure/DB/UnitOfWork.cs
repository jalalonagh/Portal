using Infrastructure.DB.Context.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using JO.Data.Base.Interfaces;

namespace Infrastructure.DB
{
    public class UnitOfWork : IUnitOfWork<MembershipEFCoreContext>
    {
        public MembershipEFCoreContext _context { get; set; }
        private IDbContextTransaction _transaction;
        public IExecutionStrategy _executionStrategy { get; private set; }

        public UnitOfWork(MembershipEFCoreContext context)
        {
            _context = context;
            _executionStrategy = context.Database.CreateExecutionStrategy();
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
        }

        public void RollbackTransaction()
        {
            _transaction?.Rollback();
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context?.Dispose();
        }

        public DbContext GetDatabase()
        {
            return _context;
        }
    }
}
