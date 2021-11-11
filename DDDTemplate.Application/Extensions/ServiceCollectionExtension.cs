using System;
using DDDTemplate.Application.Abstraction.Profile;
using DDDTemplate.Application.User;
using DDDTemplate.Infrastructure.Notification.Email;
using DDDTemplate.Infrastructure.Response;
using DDDTemplate.Infrastructure.Security.Hash;
using Microsoft.Extensions.DependencyInjection;
using DDDTemplate.Application.Abstraction.Authentication;
using DDDTemplate.Persistence.Repository;
using DDDTemplate.Persistence.Repository.Relational;
using DDDTemplate.Persistence.Repository.Mongo;

namespace DDDTemplate.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IResponseService, ResponseService>();
            services.AddScoped<IMailGunApiService, MailGunApiService>();
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            //add here new service registrations

        }

        public static void AddRepositories(this IServiceCollection services)
        {
            //For Mongo
            services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
            //For EF Core
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
