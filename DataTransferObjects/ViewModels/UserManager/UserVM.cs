using JO.Response.Base;

namespace DataTransferObjects.ViewModels.UserManager
{
    public class UserVM : BaseViewModel
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Birthday { get; set; }
        public string? Identity { get; set; }
        public string? Mobile { get; set; }
        public string? FullName { get; set; }
        public string? Avatar { get; set; }
    }
}
