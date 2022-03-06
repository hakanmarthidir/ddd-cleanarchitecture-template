using Application.Abstraction.Interfaces;
using Infrastructure.Logging;
using Infrastructure.Notification.Config;
using Infrastructure.Notification.Email;
using Infrastructure.Notification.Template;
using Infrastructure.Security.Hash;
using Infrastructure.Security.Token;
using Infrastructure.Security.Token.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.Configure<MailGunConfig>(configuration.GetSection("MailGunConfig"));
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
            services.Configure<TemplateConfig>(configuration.GetSection("TemplateConfig"));

            services.AddTransient<IEmailService, MailGunApiService>();
            services.AddTransient<ITemplateService, TemplateService>();
            services.AddTransient<IHashService, HashService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddSingleton(typeof(ILogService<>), typeof(LogService<>));
            return services;
        }
    }
}
