using ApplicationService.UserManager;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace ApplicationService.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJOServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(UserManagerProfile).Assembly);

            services.AddScoped(typeof(IJOAppService<>), typeof(JOAppService<>));

            var srvs = typeof(JOAppService<>).Assembly.GetTypes().Where(w => w.IsClass && !w.IsAbstract && !w.IsGenericType && w.GetInterface(nameof(IAppService)) != null);

            foreach (var repository in srvs)
            {
                var interfaceType = repository.GetInterfaces().FirstOrDefault(f => f.Name == $"I{repository.Name}");
                if (interfaceType != null)
                    services.AddScoped(interfaceType, repository);
            }

            return services;
        }
    }
}
