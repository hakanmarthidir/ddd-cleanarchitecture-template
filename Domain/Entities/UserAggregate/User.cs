using Ardalis.GuardClauses;
using Domain.Entities.UserAggregate.Enums;
using Domain.Entities.UserAggregate.Events;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Shared;
using System.Text.Json.Serialization;

namespace Domain.Entities.UserAggregate
{
    public class User : BaseEntity, IAggregateRoot, IAuditable, ISoftDeletable
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; private set; }
        public virtual string LastName { get; private set; }
        public virtual string Email { get; private set; }
        [JsonIgnore]
        public virtual string Password { get; private set; }
        public virtual UserTypeEnum UserType { get; private set; }
        public virtual UserActivation Activation { get; private set; }
        public virtual DateTimeOffset? CreatedDate { get; private set; }
        public virtual DateTimeOffset? ModifiedDate { get; private set; }
        public virtual string? CreatedBy { get; private set; }
        public virtual string? ModifiedBy { get; private set; }
        public virtual Status Status { get; private set; }
        public virtual DateTimeOffset? DeletedDate { get; private set; }
        public virtual string? DeletedBy { get; private set; }

        public User()
        {

        }
        private User(string firstName, string lastName, string email, string hashedPassword) : this()
        {
            FirstName = Guard.Against.NullOrWhiteSpace(firstName, nameof(firstName));
            LastName = Guard.Against.NullOrWhiteSpace(lastName, nameof(lastName));
            Email = Guard.Against.NullOrWhiteSpace(email, nameof(email));
            Password = Guard.Against.NullOrWhiteSpace(hashedPassword, nameof(hashedPassword));
            Activation = UserActivation.CreateUserActivation();
            Status = Status.Active;
            CreatedBy = $"{firstName} {lastName}";
            CreatedDate = DateTimeOffset.UtcNow;
            UserType = UserTypeEnum.User;
            this.RaiseEvent(new UserCreatedEvent(this));
        }

        public virtual void SetModifiedDate(string modifiedBy)
        {
            this.ModifiedDate = DateTimeOffset.UtcNow;
            this.ModifiedBy = modifiedBy;
        }

        public virtual void ActivateUser()
        {
            var activated = UserActivation.ActivateUser(this.Activation.ActivationCode);
            this.Activation = activated;
            SetModifiedDate(this.Id.ToString());
        }

        public virtual void CreateNewActivationCode()
        {
            this.Activation = UserActivation.CreateUserActivation();
            this.RaiseEvent(new UserActivationCodeDemandedEvent(this));
        }

        public virtual void SetPasswordAfterReset(string newPassword)
        {
            this.Password = newPassword;
        }

        public static User CreateUser(string firstName, string lastName, string email, string hashedPassword)
        {
            var newuser = new User(firstName, lastName, email, hashedPassword);

            return newuser;
        }

        public virtual bool CanActivate(string activationCode)
        {
            return this.Activation.ActivationCode == activationCode && this.Activation.IsActivated == ActivationStatusEnum.NotActivated;
        }

        public virtual void SoftDelete(string deletedBy)
        {
            this.DeletedBy = deletedBy;
            this.Status = Status.Deleted;
            this.DeletedDate = DateTimeOffset.UtcNow;
        }
    }
}
