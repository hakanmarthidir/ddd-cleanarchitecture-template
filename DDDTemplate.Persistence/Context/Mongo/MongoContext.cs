using System;
using System.Security.Authentication;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace DDDTemplate.Persistence.Context.Mongo
{
    public class MongoContext : IMongoContext
    {

        public MongoContext(IConfiguration configuration)
        {
            var mongoConnectionString = configuration.GetConnectionString("MongoConnection");
            var url = new MongoUrl(mongoConnectionString);
            MongoClientSettings settings = MongoClientSettings.FromUrl(url);
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var client = new MongoClient(settings);
            Database = client.GetDatabase(url.DatabaseName);
        }
        public IMongoDatabase Database { get; set; }

    }
}
