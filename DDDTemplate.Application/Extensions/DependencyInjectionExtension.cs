using DDDTemplate.Application.User;
using Microsoft.Extensions.DependencyInjection;
using DDDTemplate.Application.Abstraction.User;

namespace DDDTemplate.Application.Extensions
{
    public static class DependencyInjectionExtension
    {

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Mappers.AutoMappings));
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            return services;
        }
    }
}
