using Microsoft.AspNetCore.Http;
using JO.Shared.Interfaces;

namespace JO.Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ITokenService _tokenService;
        public long? UserId { get; protected set; }

        public CurrentUserService(IHttpContextAccessor http, ITokenService tokenService)
        {
            _tokenService = tokenService;

            string token = (http?.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault() ?? "").Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                UserId = _tokenService.ValidateToken(token);
            }
        }
    }
}
