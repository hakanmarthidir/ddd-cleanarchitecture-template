using Domain.Shared;

namespace Application.User.Events
{
    public class UserCreatedEvent : DomainEvent
    {
        public Domain.Entities.UserAggregate.User CreatedUser { get; }

        public UserCreatedEvent(Domain.Entities.UserAggregate.User user)
        {
            this.CreatedUser = user;
        }
    }
}
