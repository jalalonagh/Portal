using Core.UserManager;
using Infrastructure.DB;
using Infrastructure.DB.Context.EFCore;
using Microsoft.Extensions.Configuration;
using JO.Data.Base.Interfaces;
using JO.Shared.Interfaces;

namespace Infrastructure.Repositories
{
    public class UserRepository : EfCoreRepository<User>, IUserRepository, IJORepository
    {
        public UserRepository(MembershipEFCoreContext context,
            IUnitOfWork<MembershipEFCoreContext> unitOfWork,
            ICurrentUserService currentUser,
            IConfiguration configuration) : base(context, unitOfWork, currentUser, configuration)
        {
        }
    }
}

