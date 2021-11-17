using System;
using System.Text.Json.Serialization;
using DDDTemplate.Domain.AggregatesModel.UserAggregate.Enums;
using DDDTemplate.Domain.Enums;
using DDDTemplate.Domain.Interfaces;

namespace DDDTemplate.Domain.AggregatesModel.UserAggregate
{
    public class User : IEntity, IAuditable, ISoftDelete
    {
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

        public static User CreateUser(string firstName, string lastName, string email, string hashedPassword)
        {
            var newuser = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = hashedPassword,
                IsActivated = ActivationStatus.NotActivated,
                Status = Status.Active,
                CreatedDate = DateTimeOffset.UtcNow,
                UserType = UserType.User,
                ActivationCode = Guid.NewGuid().ToString()
            };

            return newuser;

        }

    }
}
