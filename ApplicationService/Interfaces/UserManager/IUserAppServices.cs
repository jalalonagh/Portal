using Core.UserManager;
using DataTransferObjects.InputModels.UserManager.Users;
using DataTransferObjects.ViewModels.UserManager;

namespace ApplicationService.UserManager
{
    public interface IUserAppServices
    {
        Task<UserVM?> FindAsync(CancellationToken cancellation, long id);
        Task<LoginVM> LoginAsync(CancellationToken cancellation, LoginDTO dto);
        Task<string> LoginSwaggerAsync(CancellationToken cancellation, LoginSwaggerDTO dto);
    }
}
