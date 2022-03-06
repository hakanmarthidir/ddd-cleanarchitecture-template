using Domain.Shared;

namespace Domain.Entities.UserAggregate.Events
{
    public class UserCreatedEvent : DomainEvent
    {
        public User CreatedUser { get; }

        public UserCreatedEvent(User user)
        {
            this.CreatedUser = user;
        }
    }
}
