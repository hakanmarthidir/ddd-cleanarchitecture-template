using System;
using DDDTemplate.Domain.Entities.UserAggregate;
using DDDTemplate.Domain.Entities.UserAggregate.Enums;
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
                map.SetIdMember(map.GetMemberMap(c => c.Id));
                map.MapMember(x => x.Email).SetIsRequired(true);
                map.MapMember(x => x.FirstName).SetIsRequired(true);
                map.MapMember(x => x.LastName).SetIsRequired(true);
                map.MapMember(x => x.Password).SetIsRequired(true);
                map.MapMember(x => x.UserType).SetSerializer(new SmartEnumMongoSerializer<UserTypeEnum>());
                map.MapMember(x => x.Activation);
            });

            BsonClassMap.RegisterClassMap<UserActivation>(map =>
            {
                map.AutoMap();
                map.SetIgnoreExtraElements(true);
                map.MapMember(p =>p.IsActivated).SetSerializer(new SmartEnumMongoSerializer<ActivationStatusEnum>());
            });
        }
    }
}
