using JO.Data.Base.Interfaces;

namespace Core.UserManager
{
    public class UserDomain : DomainManager<User>, IDomain
    {
        public UserDomain(IEfCoreRepository<User> _repository) : base(_repository)
        {

        }
    }
}
