using MongoDB.Driver;

namespace DDDTemplate.Persistence.Context.Mongo
{
    public interface IMongoContext
    {
        IMongoDatabase Database { get; set; }
    }
}
