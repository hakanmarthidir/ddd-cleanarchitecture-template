using Ardalis.GuardClauses;
using Domain.Entities.UserAggregate.Enums;
using Domain.Shared;

namespace Domain.Entities.UserAggregate
{
    public class UserActivation : ValueObject
    {
        public virtual ActivationStatusEnum IsActivated { get; private set; }
        public virtual string ActivationCode { get; private set; }
        public virtual DateTimeOffset? ActivationDate { get; private set; }
        private UserActivation(ActivationStatusEnum activationStatusEnum, string activationCode, DateTimeOffset? activationDate)
        {
            IsActivated = activationStatusEnum;
            ActivationCode = activationCode;
            ActivationDate = activationDate;
        }

        public static UserActivation CreateUserActivation()
        {
            var newUserActivation = new UserActivation(ActivationStatusEnum.NotActivated, Guid.NewGuid().ToString(), null);           

            return newUserActivation;
        }

        public static UserActivation ActivateUser(string activationCode)
        {
            Guard.Against.NullOrWhiteSpace(activationCode, nameof(activationCode), "ActivationCode could not be null.");
            var activatedModel = new UserActivation(ActivationStatusEnum.Activated, activationCode, DateTimeOffset.UtcNow);           

            return activatedModel;

        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ActivationCode;
            yield return IsActivated;
        }
    }
}
