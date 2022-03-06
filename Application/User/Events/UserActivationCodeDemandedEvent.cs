using Domain.Shared;

namespace Application.User.Events
{
    public class UserActivationCodeDemandedEvent : DomainEvent
    {
        public Domain.Entities.UserAggregate.User DemandedUser { get; }

        public UserActivationCodeDemandedEvent(Domain.Entities.UserAggregate.User user)
        {
            this.DemandedUser = user;
        }
    }
}
