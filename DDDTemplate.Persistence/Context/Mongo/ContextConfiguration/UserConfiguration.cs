using System;
using DDDTemplate.Domain.AggregatesModel.UserAggregate;
using MongoDB.Bson.Serialization;

namespace DDDTemplate.Persistence.Context.Mongo.ContextConfiguration
{
    public class UserMap
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<User>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapIdMember(x => x.Id);
                map.MapMember(x => x.Email).SetIsRequired(true);
                map.MapMember(x => x.FirstName).SetIsRequired(true);
                map.MapMember(x => x.LastName).SetIsRequired(true);
                map.MapMember(x => x.Password).SetIsRequired(true);
            });
        }
    }
}
