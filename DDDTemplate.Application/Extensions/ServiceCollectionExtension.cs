using System;
using DDDTemplate.Application.User;
using DDDTemplate.Infrastructure.Notification.Email;
using DDDTemplate.Infrastructure.Security.Hash;
using Microsoft.Extensions.DependencyInjection;
using DDDTemplate.Persistence.Repository.User;
using DDDTemplate.Domain.Entities.UserAggregate;
using DDDTemplate.Persistence.Context.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DDDTemplate.Persistence.Context.Mongo;
using DDDTemplate.Infrastructure.Security.Token;
using DDDTemplate.Persistence.Context.Relational.Uow;
using DDDTemplate.Persistence.Context.Mongo.ContextConfiguration;
using DDDTemplate.Application.Abstraction.User;
using DDDTemplate.Infrastructure.Notification.Template;
using DDDTemplate.Domain.Interfaces;

namespace DDDTemplate.Application.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static void AddMySqlDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var serverVersion = new MariaDbServerVersion(new Version(10, 5, 8));

            services.AddDbContext<EFContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(configuration.GetConnectionString("MariaDbConnection"), serverVersion)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

            services.AddScoped<IUnitOfWork, EFUnitOfWork>();
        }

        public static void AddMongoDatabaseContext(this IServiceCollection services)
        {
            MongoDbPersistence.Configure();
            services.AddScoped<IMongoContext, MongoContext>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Mappers.AutoMappings));
            services.AddScoped<IMailGunApiService, MailGunApiService>();
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ITemplateService, TemplateService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
    }
}
