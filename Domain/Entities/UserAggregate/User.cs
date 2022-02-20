using Ardalis.GuardClauses;
using Domain.Entities.UserAggregate.Enums;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using System.Text.Json.Serialization;

namespace Domain.Entities.UserAggregate
{
    public class User : BaseEntity<Guid>, IAggregateRoot, IAuditable, ISoftDeletable
    {
        public virtual string FirstName { get; private set; }
        public virtual string LastName { get; private set; }
        public virtual string Email { get; private set; }
        [JsonIgnore]
        public virtual string Password { get; private set; }
        public virtual UserTypeEnum UserType { get; private set; }
        public virtual UserActivation Activation { get; private set; }
        public virtual DateTimeOffset? CreatedDate { get;  set; }
        public virtual DateTimeOffset? ModifiedDate { get;  set; }
        public virtual string? CreatedBy { get; set; }
        public virtual string? ModifiedBy { get; set; }
        public virtual Status Status { get;  set; }
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
                FirstName = Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName)),
                LastName = Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName)),
                Email = Guard.Against.NullOrWhiteSpace(email, nameof(email)),
                Password = Guard.Against.NullOrWhiteSpace(hashedPassword, nameof(hashedPassword)),
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
