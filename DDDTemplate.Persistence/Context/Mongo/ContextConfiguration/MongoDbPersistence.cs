using MongoDB.Bson.Serialization.Conventions;

namespace DDDTemplate.Persistence.Context.Mongo.ContextConfiguration
{
    public static class MongoDbPersistence
    {
        public static void Configure()
        {
            UserMap.Configure();

            // Conventions
            var pack = new ConventionPack
                {
                    new IgnoreExtraElementsConvention(true),
                    new IgnoreIfDefaultConvention(true)
                };
            ConventionRegistry.Register("DDDTemplate Conventions", pack, t => true);
        }
    }
}
