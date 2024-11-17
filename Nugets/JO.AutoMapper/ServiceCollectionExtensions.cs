using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace JO.AutoMapper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJOAutoMapperServices(this IServiceCollection services, Assembly asm)
        {
            services.AddAutoMapper(asm);

            services.AddSingleton<IJOMapper, JOMapper>();

            return services;
        }
    }
}
