using JO.Response.Base;

namespace DataTransferObjects.ViewModels.UserManager.TempUser
{
    public class TempUserVM : BaseViewModel
    {
        public string Birthday { get; set; }
        public string Identity { get; set; }
        public string Mobile { get; set; }
        public string PersonalData { get; set; }
        public bool? IsReserved { get; set; }
        public string Avatar { get; set; }
        public int Status { get; set; }
    }
}
