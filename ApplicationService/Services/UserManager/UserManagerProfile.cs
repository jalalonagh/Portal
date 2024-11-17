using AutoMapper;
using Core.UserManager;
using DataTransferObjects.ViewModels.UserManager;
using DataTransferObjects.ViewModels.UserManager.TempUser;

namespace ApplicationService.UserManager
{
    public class UserManagerProfile : Profile
    {
        public UserManagerProfile()
        {
            CreateMap<User, UserVM>()
                .ForMember(t => t.Identity, s => s.MapFrom(src => src.Identity.Identity))
                .ForMember(t => t.Avatar, s => s.MapFrom(src => src.Avatar.ThumbnailPath))
                .ForMember(t => t.FullName, s => s.MapFrom(src => src.PersonalData.ToString()))
                .ForMember(t => t.Birthday, s => s.MapFrom(src => src.Birthday.ToString()))
                .ForMember(t => t.Mobile, s => s.MapFrom(src => src.Mobile.Number));
        }
    }
}
