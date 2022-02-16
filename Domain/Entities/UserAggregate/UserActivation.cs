using Domain.Entities.UserAggregate.Enums;
using Domain.Shared;

namespace Domain.Entities.UserAggregate
{
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
