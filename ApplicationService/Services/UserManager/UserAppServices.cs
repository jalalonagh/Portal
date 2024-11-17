using Core;
using Core.UserManager;
using DataTransferObjects.InputModels.UserManager.Users;
using DataTransferObjects.ViewModels.UserManager;
using JO.AutoMapper;
using JO.Data.Base.Interfaces;
using JO.Shared.Interfaces;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Identity;

namespace ApplicationService.UserManager
{
    public class UserAppServices : IUserAppServices
    {
        private readonly IJOMapper _mapper;
        private readonly UserManager<User> _manager;
        private readonly UserDomain _domain;
        private readonly ITokenService _tokenService;
        private readonly IEfCoreRepository<User> repo;

        public UserAppServices(IJOMapper mapper,
            DomainManager<User> _domainManager,
            IEfCoreRepository<User> _repo,
            UserDomain domain,
            UserManager<User> manager,
            ITokenService tokenService)
        {
            _domain = domain;
            _manager = manager;
            _tokenService = tokenService;
            repo = _repo;
            _mapper = mapper;
        }

        public async Task<UserVM?> FindAsync(CancellationToken cancellation, long id)
        {
            var result = await repo.FindAsync(cancellation, f => f.Id == id);

            return _mapper.Map<UserVM>(result);
        }

        public async Task<LoginVM> LoginAsync(CancellationToken cancellation, LoginDTO dto)
        {
#if RELEASE
                var captchaIsChecked = await _textCaptchaAppService.CheckCaptcha(cancellation, dto.CaptchaId, dto.CaptchaValue);
                if (captchaIsChecked == false)
                    throw new Exception("CaptchaIsFalse");
#endif

            var user = await _domain.FindAsync(cancellation, f => f.UserName == dto.UserName);

            if (user == null)
            {
                throw new Exception("کاربر یافت نشد");
            }

            var passResult = await _manager.CheckPasswordAsync(user, dto.Password);

            if (!passResult)
            {
                throw new Exception("رمز ارسالی یا شناسه کاربری صحیح نیست");
            }

            var token = _tokenService.CreateToken(dto.UserName, user.Id);

            return new LoginVM()
            {
                Token = token,
            };
        }

        public async Task<string> LoginSwaggerAsync(CancellationToken cancellation, LoginSwaggerDTO dto)
        {
            var user = await _domain.FindAsync(cancellation, f => f.UserName == dto.UserName);

            if (user == null)
            {
                throw new Exception("کاربر یافت نشد");
            }

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
