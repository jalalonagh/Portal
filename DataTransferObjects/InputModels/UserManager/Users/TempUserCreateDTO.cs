using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects.InputModels.UserManager.Users
{
    public class TempUserCreateDTO
    {
        [Required]
        [StringLength(10, ErrorMessage = "کد ملی باید 10 رقم باشد")]
        public string NationalIdentity { get; set; } = null!;
        [Required]
        [StringLength(15, ErrorMessage = "شماره موبایل نمی تواند بیشتر از 15 کاراکتر باشد")]
        public string MobileNumber { get; set; } = null!;
        [Required]
        [MaxLength(10, ErrorMessage = "فرمت تاریخ به صورت yyyy-MM-dd می باشد")]
        public string Birthday { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string PostalCode { get; set; }

        [Required]
        public long CaptchaId { get; set; }
        [Required]
        public string CaptchaValue { get; set; }
    }
}
