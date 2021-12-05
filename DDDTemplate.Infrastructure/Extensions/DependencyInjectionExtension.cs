using DDDTemplate.Application.Abstraction.External;
using DDDTemplate.Infrastructure.Logging;
using DDDTemplate.Infrastructure.Notification.Config;
using DDDTemplate.Infrastructure.Notification.Email;
using DDDTemplate.Infrastructure.Notification.Template;
using DDDTemplate.Infrastructure.Security.Hash;
using DDDTemplate.Infrastructure.Security.Token;
using DDDTemplate.Infrastructure.Security.Token.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DDDTemplate.Infrastructure.Extensions
{
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.Configure<MailGunConfig>(configuration.GetSection("EmailConfig"));
            services.Configure<JwtConfig>(configuration.GetSection("JwtConfig"));
            services.Configure<TemplateConfig>(configuration.GetSection("TemplateConfig"));

            services.AddTransient<IEmailService, MailGunApiService>();
            services.AddTransient<ITemplateService, TemplateService>();
            services.AddTransient<IHashService, HashService>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddScoped(typeof(ILogService<>), typeof(LogService<>));
            return services;
        }
    }
}
