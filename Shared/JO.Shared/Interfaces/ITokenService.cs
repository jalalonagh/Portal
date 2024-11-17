using System.Security.Claims;

namespace JO.Shared.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(string userName, long userId);
        string CreateToken(string username, string password);
        ClaimsPrincipal? Validate(string token);
        long? ValidateToken(string token);
    }
}
