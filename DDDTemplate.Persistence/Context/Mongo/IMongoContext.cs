using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DDDTemplate.Persistence.Context.Mongo
{
    public interface IMongoContext : IDisposable
    {
        IMongoDatabase Database { get; set; }
    }
}
