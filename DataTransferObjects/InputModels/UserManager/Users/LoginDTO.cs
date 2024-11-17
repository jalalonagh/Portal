using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects.InputModels.UserManager.Users
{
    public class LoginDTO
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public long CaptchaId { get; set; }
        [Required]
        public string CaptchaValue { get; set; }
    }
}
