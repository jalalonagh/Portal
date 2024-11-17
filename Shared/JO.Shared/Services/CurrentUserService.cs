using Microsoft.AspNetCore.Http;
using JO.Shared.Interfaces;

namespace JO.Shared.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public long? UserId { get; protected set; }

        public CurrentUserService(IHttpContextAccessor http, ITokenService tokenService)
        {
            string token = (http?.HttpContext?.Request?.Headers["Authorization"].FirstOrDefault() ?? "").Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                UserId = tokenService.ValidateToken(token);
            }
        }
    }
}
