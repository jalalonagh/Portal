using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects.InputModels.UserManager.Users
{
    public class RegisterDTO
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
