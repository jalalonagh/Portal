using Core;
using Core.UserManager;
using DataTransferObjects.InputModels.UserManager.Users;
using DataTransferObjects.ViewModels.UserManager;
using JO.AutoMapper;
using JO.Data.Base.Interfaces;
using JO.Shared.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ApplicationService.UserManager
{
    public class UserAppServices : JOAppService<User>, IUserAppServices
    {
        private readonly UserManager<User> _manager;
        private readonly UserDomain _domain;
        private readonly ITokenService _tokenService;

        public UserAppServices(IJOMapper _mapper,
            DomainManager<User> _domainManager,
            IEfCoreRepository<User> _repo,
            UserDomain domain,
            UserManager<User> manager,
            ITokenService tokenService) : base(_mapper, _repo, _domainManager)
        {
            _domain = domain;
            _manager = manager;
            _tokenService = tokenService;
        }

        public async Task<LoginVM> LoginAsync(CancellationToken cancellation, LoginDTO dto)
        {
#if RELEASE
                var captchaIsChecked = await _textCaptchaAppService.CheckCaptcha(cancellation, dto.CaptchaId, dto.CaptchaValue);
                if (captchaIsChecked == false)
                    throw new Exception("CaptchaIsFalse");
#endif

            var user = await _domain.FindAsync(cancellation, f => f.UserName == dto.UserName);

            if (user == null) throw new Exception("کاربر یافت نشد");

            var passResult = await _manager.CheckPasswordAsync(user, dto.Password);

            if (passResult)
            {
                var token = _tokenService.CreateToken(dto.UserName, user.Id);

                //var menus = await _activityAppServices.GetMenusByActivityAsync(cancellation, 6);

                return new LoginVM()
                {
                    Token = token,
                    //User = mapper.Map<UserVM>(user),
                    //Menus = menus
                };
            }

            throw new Exception("رمز ارسالی یا شناسه کاربری صحیح نیست");
        }

        public async Task<string> LoginSwaggerAsync(CancellationToken cancellation, LoginSwaggerDTO dto)
        {
            var user = await _domain.FindAsync(cancellation, f => f.UserName == dto.UserName);

            if (user == null) throw new Exception("کاربر یافت نشد");

            var passResult = await _manager.CheckPasswordAsync(user, dto.Password);

            if (passResult)
            {
                var token = _tokenService.CreateToken(dto.UserName, user.Id);

                return token;
            }

            throw new Exception("رمز ارسالی یا شناسه کاربری صحیح نیست");
        }
    }
}
