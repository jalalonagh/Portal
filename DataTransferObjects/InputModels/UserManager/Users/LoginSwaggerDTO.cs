using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects.InputModels.UserManager.Users
{
    public class LoginSwaggerDTO
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
