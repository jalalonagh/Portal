using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace JO.Data.Base.Interfaces
{
    public interface IUnitOfWork<DB> : IDisposable
        where DB : DbContext
    {
        IExecutionStrategy _executionStrategy { get; }
        DB _context { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        DbContext GetDatabase();
    }
}
