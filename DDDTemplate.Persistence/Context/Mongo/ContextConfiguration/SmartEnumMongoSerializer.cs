using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json;

namespace DDDTemplate.Persistence.Context.Mongo.ContextConfiguration
{
    public class SmartEnumMongoSerializer<T> : SerializerBase<T>
    {
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, T value)
        {
            var jsonDocument = JsonConvert.SerializeObject(value);
            var bsonDocument = BsonSerializer.Deserialize<BsonDocument>(jsonDocument);

            var serializer = BsonSerializer.LookupSerializer(typeof(BsonDocument));
            serializer.Serialize(context, bsonDocument.AsBsonValue);
        }

        public override T Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var serializer = BsonSerializer.LookupSerializer(typeof(BsonDocument));
            var document = serializer.Deserialize(context, args);

            var bsonDocument = document.ToBsonDocument();

            var result = BsonExtensionMethods.ToJson(bsonDocument);
            var deserializedEnum = JsonConvert.DeserializeObject<T>(result);
            return deserializedEnum;
        }
    }
}
