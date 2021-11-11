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
using DDDTemplate.Domain.SeedWork;
using DDDTemplate.Persistence.Repository.User;
using DDDTemplate.Domain.AggregatesModel.UserAggregate;
using DDDTemplate.Persistence.Context.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using DDDTemplate.Persistence.Context.Mongo;
using DDDTemplate.Infrastructure.Notification.Config;

namespace DDDTemplate.Application.Extensions
{
    public static class ServiceCollectionExtension
    {

        public static void AddConfigs(this IServiceCollection services, IConfiguration configuration)
        {

        }

        public static void AddMySqlDatabaseContext(this IServiceCollection services, IConfiguration configuration)
        {
            var serverVersion = new MariaDbServerVersion(new Version(10, 5, 8));

            services.AddDbContext<EFContext>(
                dbContextOptions => dbContextOptions
                    .UseMySql(configuration.GetConnectionString("MariaDbConnection"), serverVersion)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());

        }

        public static void AddMongoDatabaseContext(this IServiceCollection services)
        {
            services.AddScoped<IMongoContext, MongoContext>();
        }

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

            services.AddScoped<IUserRepository, UserRepository>();

            //Use For Mongo
            //services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));

            //Use For EF Core
            //services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
