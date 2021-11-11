using System.Security.Authentication;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DDDTemplate.Persistence.Context.Mongo
{
    public class MongoContext : IMongoContext
    {
        private readonly MongoConfig _mongoConfig;

        public MongoContext(IOptionsMonitor<MongoConfig> mongoConfig)
        {
            this._mongoConfig = mongoConfig.CurrentValue;
            var url = new MongoUrl(this._mongoConfig.ConnectionString);
            MongoClientSettings settings = MongoClientSettings.FromUrl(url);
            //settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var client = new MongoClient(settings);
            Database = client.GetDatabase(url.DatabaseName);
        }
        public IMongoDatabase Database { get; set; }

    }
}
