using System.ComponentModel.DataAnnotations;

namespace DataTransferObjects.InputModels.UserManager.Users
{
    public class TempUserUpdateDTO
    {
        [StringLength(10, ErrorMessage = "کد ملی باید 10 رقم باشد")]
        public string? NationalIdentity { get; set; } = null!;
        [StringLength(15, ErrorMessage = "شماره موبایل نمی تواند بیشتر از 15 کاراکتر باشد")]
        public string? MobileNumber { get; set; } = null!;
        [StringLength(10, ErrorMessage = "فرمت تاریخ به صورت yyyy-MM-dd می باشد")]
        public string? Birthday { get; set; }

        [StringLength(50, ErrorMessage = "نام نمی توان بیشتر از 50 کاراکتر باشد")]
        public string? FirstName { get; set; }
        [StringLength(50, ErrorMessage = "نام خانوادگی نمی تواند بیشتر از 50 کاراکتر باشد")]
        public string? LastName { get; set; }
    }
}
