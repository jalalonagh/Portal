
namespace DataTransferObjects.ViewModels.UserManager
{
    public class LoginVM
    {
        public string Token { get; set; }
        public UserVM User { get; set; }
        public List<string>? Roles { get; set; }
        public List<string>? Permissions { get; set; }
    }
}
