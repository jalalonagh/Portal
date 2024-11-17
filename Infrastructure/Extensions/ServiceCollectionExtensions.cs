using Infrastructure.DB;
using Infrastructure.DB.Context.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using JO.Data.Base.Interfaces;
using System.Data;

namespace Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJODbContext(this IServiceCollection services, IConfigurationManager configuration)
        {
            services.AddDbContext<MembershipEFCoreContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            }, ServiceLifetime.Transient);

            return services;
        }

        public static IServiceCollection AddJORepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IEfCoreRepository<>), typeof(EfCoreRepository<>));

            var repos = typeof(EfCoreRepository<>).Assembly.GetTypes().Where(w => w.IsClass && !w.IsAbstract && w.GetInterface(nameof(IJORepository)) != null);
            foreach (var repository in repos)
            {
                var interfaceType = repository.GetInterfaces().FirstOrDefault(f => f.Name == $"I{repository.Name}");
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, repository);
                }
            }

            return services;
        }
    }
}
