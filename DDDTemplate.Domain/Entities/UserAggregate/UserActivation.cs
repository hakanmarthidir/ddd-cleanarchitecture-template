using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ardalis.SmartEnum.JsonNet;
using DDDTemplate.Domain.Entities.UserAggregate.Enums;
using DDDTemplate.Domain.Shared;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;

namespace DDDTemplate.Domain.Entities.UserAggregate
{
    [BsonIgnoreExtraElements]
    public class UserActivation : ValueObject
    {    
        public virtual ActivationStatusEnum IsActivated { get; set; }
        public virtual string ActivationCode { get; set; }
        public virtual DateTimeOffset? ActivationDate { get; set; }
        public UserActivation()
        {           
        }

        public static UserActivation CreateUserActivation()
        {
            var newUserActivation = new UserActivation()
            {
                IsActivated = ActivationStatusEnum.NotActivated,
                ActivationCode = Guid.NewGuid().ToString()
            };

            return newUserActivation;

        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ActivationCode;
            yield return IsActivated;
        }
    }
}
