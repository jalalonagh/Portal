using Core.UserManager;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Core
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJODomainServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(DomainManager<>));

            var dmns = typeof(UserDomain).Assembly.GetTypes().Where(w => w.IsClass && !w.IsGenericType && !w.IsAbstract && w.GetInterface(nameof(IDomain)) != null);

            foreach (var manager in dmns)
            {
                services.AddScoped(manager);
            }

            return services;
        }
    }
}
