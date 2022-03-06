using Domain.Shared;

namespace Domain.Entities.UserAggregate.Events
{
    public class UserActivationCodeDemandedEvent : DomainEvent
    {
        public User DemandedUser { get; }

        public UserActivationCodeDemandedEvent(User user)
        {
            this.DemandedUser = user;
        }
    }
}
