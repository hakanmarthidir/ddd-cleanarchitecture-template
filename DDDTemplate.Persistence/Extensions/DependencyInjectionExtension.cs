using System;
using DDDTemplate.Domain.Entities.UserAggregate;
using DDDTemplate.Persistence.Context.Mongo;
using DDDTemplate.Persistence.Context.Mongo.ContextConfiguration;
using DDDTemplate.Persistence.Repository.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace DDDTemplate.Infrastructure.Extensions
{

    //public class GuidSerializationProvider : IBsonSerializationProvider
    //{
    //    public IBsonSerializer GetSerializer(Type type)
    //    {
    //        return type == typeof(Guid) ? new GuidSerializer(GuidRepresentation.CSharpLegacy) : null;
    //    }
    //}
    public static class DependencyInjectionExtension
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {

            //----MONGO REGISTRATION -----
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
            //BsonSerializer.RegisterSerializationProvider(new GuidSerializationProvider());
            MongoDbPersistence.Configure();
            services.AddScoped<IMongoContext, MongoContext>();

            //----EF REGISTRATION ----
            //var serverVersion = new MariaDbServerVersion(new Version(10, 5, 8));
            //services.AddDbContext<EFContext>(
            //    dbContextOptions => dbContextOptions
            //        .UseMySql(configuration.GetConnectionString("MariaDbConnection"), serverVersion)
            //    .EnableSensitiveDataLogging()
            //    .EnableDetailedErrors());
            //services.AddScoped<IUnitOfWork, EFUnitOfWork>();

            //REPOSITORIES ------
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
