using Infrastructure.DB.Context.EFCore;
using Microsoft.EntityFrameworkCore;
using JO.Data.Base.Interfaces;

namespace Api.Middlewares
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IUnitOfWork<MembershipEFCoreContext> unitOfWork)
        {
            await unitOfWork._executionStrategy.ExecuteAsync(async () =>
            {
                try
                {
                    unitOfWork.BeginTransaction();

                    await _next(context);

                    var result = await unitOfWork.SaveChangesAsync(default);
                    unitOfWork.CommitTransaction();

                    return Task.CompletedTask;
                }
                catch (Exception ex)
                {
                    unitOfWork.RollbackTransaction();
                    throw;
                }
            });
        }
    }
}
