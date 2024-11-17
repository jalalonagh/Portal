using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JO.Shared.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JO.Shared.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string CreateToken(string username, long userId)
        {
            var _Issuer = _configuration["Jwt:Issuer"];
            var _Audience = _configuration["Jwt:Audience"];
            var _Key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var securityKey = new SymmetricSecurityKey(_Key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, username ?? ""));

            var token = new JwtSecurityToken(_Issuer, _Audience, claims, expires: DateTime.Now.AddMinutes(10000), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateToken(string username, string password)
        {
            var _Issuer = _configuration["Jwt:Issuer"];
            var _Audience = _configuration["Jwt:Audience"];
            var _Key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var securityKey = new SymmetricSecurityKey(_Key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, username ?? ""));

            var token = new JwtSecurityToken(_Issuer, _Audience, claims, expires: DateTime.Now.AddMinutes(10000), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public long? ValidateToken(string token)
        {
            var _SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var _Issuer = _configuration["Jwt:Issuer"];
            var _Audience = _configuration["Jwt:Audience"];
            var _Key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            if (string.IsNullOrEmpty(token) || string.IsNullOrWhiteSpace(token))
            {
                return 0;
            }

            var tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _SymmetricSecurityKey,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidAudience = _Audience,
                ValidIssuer = _Issuer,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var id = jwtToken?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            if (id != null && long.TryParse(id.Value, out long uid))
            {
                return long.Parse(id.Value ?? "0");
            }

            return 0;
        }

        public ClaimsPrincipal? Validate(string token)
        {
            var _SymmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var _Issuer = _configuration["Jwt:Issuer"];
            var _Audience = _configuration["Jwt:Audience"];
            var _Key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

            var tokenHandler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _Issuer,
                ValidAudience = _Audience,
                IssuerSigningKey = new SymmetricSecurityKey(_Key)
            };

            SecurityToken validatedToken;
            if (!string.IsNullOrEmpty(token))
            {
                if (tokenHandler.TokenLifetimeInMinutes <= 0)
                {
                    return null;
                }

                if (!tokenHandler.CanReadToken(token))
                {
                    return null;
                }

                var tokenData = tokenHandler.ReadToken(token);

                if (tokenData.ValidTo <= DateTime.Now)
                {
                    return null;
                }

                return tokenHandler.ValidateToken(token, parameters, out validatedToken);
            }

            return null;
        }
    }
}
