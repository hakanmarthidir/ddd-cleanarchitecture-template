using Application.User;
using Microsoft.Extensions.DependencyInjection;
using Application.Abstraction.User;
using MediatR;
using System.Reflection;

namespace Application.Extensions
{
    public static class DependencyInjectionExtension
    {

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(Mappers.AutoMappings));
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();            
            return services;
        }
    }
}
