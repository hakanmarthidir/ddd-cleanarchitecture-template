using Domain.Entities.UserAggregate.Enums;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using System.Text.Json.Serialization;

namespace Domain.Entities.UserAggregate
{
    public class User : BaseEntity, IAggregateRoot, IAuditable, ISoftDeletable
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        [JsonIgnore]
        public virtual string Password { get; set; }
        public virtual UserTypeEnum UserType { get; set; }
        public virtual UserActivation Activation { get; set; }
        public virtual DateTimeOffset? CreatedDate { get; set; }
        public virtual DateTimeOffset? ModifiedDate { get; set; }
        public virtual string? CreatedBy { get; set; }
        public virtual string? ModifiedBy { get; set; }
        public virtual Status Status { get; set; }
        public virtual DateTimeOffset? DeletedDate { get; set; }
        public virtual string? DeletedBy { get; set; }        

        public virtual void SetDeletedDate(string deletedBy)
        {
            this.DeletedDate = DateTimeOffset.UtcNow;
            this.DeletedBy = deletedBy;
        }

        public virtual void SetModifiedDate(string modifiedBy)
        {
            this.ModifiedDate = DateTimeOffset.UtcNow;
            this.ModifiedBy = modifiedBy;
        }

        public virtual void ActivateUser()
        {
            this.Activation.IsActivated = ActivationStatusEnum.Activated;
            this.Activation.ActivationDate = DateTimeOffset.UtcNow;
            SetModifiedDate(this.Id.ToString());
        }

        public virtual void CreateActivationCode()
        {
            this.Activation.ActivationCode = Guid.NewGuid().ToString();
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
                Activation = UserActivation.CreateUserActivation(),
                Status = Status.Active,
                CreatedBy = $"{firstName} {lastName}",
                CreatedDate = DateTimeOffset.UtcNow,
                UserType = UserTypeEnum.User
            };

            return newuser;

        }

        public virtual bool CanActivate(string activationCode)
        {
            return this.Activation.ActivationCode == activationCode && this.Activation.IsActivated == ActivationStatusEnum.NotActivated;
        }

    }
}
