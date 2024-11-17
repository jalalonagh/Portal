using ApplicationService.UserManager;
using DataTransferObjects.InputModels.UserManager.Users;
using DataTransferObjects.ViewModels.UserManager;
using JO.Response.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedWebApi.AuthorizeValidation;
using SharedWebApi.Controllers;

namespace Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : BaseApiController
    {
        private readonly IUserAppServices _services;

        public UserController(IUserAppServices services)
        {
            _services = services;
        }

        [HttpGet("[action]")]
        public async Task<ApiResult<UserVM?>> GetUserByTokenAsync(CancellationToken cancellation)
        {
            return await _services.FindAsync(cancellation, UserId).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<ApiResult<LoginVM>> LoginAsync(CancellationToken cancellation, LoginDTO dto)
        {
            return await _services.LoginAsync(cancellation, dto).ConfigureAwait(true);
        }

        [HttpPost]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<string> LoginSwaggerAsync(CancellationToken cancellation, LoginSwaggerDTO dto)
        {
            return await _services.LoginSwaggerAsync(cancellation, dto).ConfigureAwait(true);
        }
    }
}
