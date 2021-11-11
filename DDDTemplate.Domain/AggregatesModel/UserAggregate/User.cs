using System;
using System.Text.Json.Serialization;
using DDDTemplate.Domain.AggregatesModel.UserAggregate.Enums;
using DDDTemplate.Domain.SeedWork;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DDDTemplate.Domain.AggregatesModel.UserAggregate
{
    public class User : IEntity, IAuditable, ISoftDelete
    {
        //Open if you use MongoContext
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        public virtual Guid Id { get; set; }
        public virtual DateTimeOffset? CreatedDate { get; set; }
        public virtual DateTimeOffset? ModifiedDate { get; set; }
        public virtual Status Status { get; set; }
        public virtual DateTimeOffset? DeletedDate { get; set; }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        [JsonIgnore]
        public virtual string Password { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual ActivationStatus IsActivated { get; set; }
        public virtual string ActivationCode { get; set; }
        public virtual DateTimeOffset? ActivationDate { get; set; }


        public virtual void SetDeletedDate()
        {
            this.DeletedDate = DateTimeOffset.UtcNow;
        }

        public virtual void SetModifiedDate()
        {
            this.ModifiedDate = DateTimeOffset.UtcNow;
        }

        public virtual void ActivateUser()
        {
            this.IsActivated = ActivationStatus.Activated;
            this.ActivationDate = DateTimeOffset.UtcNow;
            SetModifiedDate();
        }

        public virtual void CreateActivationCode()
        {
            this.ActivationCode = Guid.NewGuid().ToString();
        }

        public virtual void SetPasswordAfterReset(string newPassword)
        {
            this.Password = newPassword;
        }

    }
}
