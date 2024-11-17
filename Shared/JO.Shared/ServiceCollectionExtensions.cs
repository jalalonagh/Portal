using Microsoft.Extensions.DependencyInjection;
using JO.Shared.Interfaces;
using JO.Shared.Services;

namespace JO.Shared
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddScoped<ITokenService, TokenService>();

            services.AddScoped<ICurrentUserService, CurrentUserService>();

            return services;
        }
    }
}
