using MediatR;

namespace Domain.Shared
{
    public abstract class DomainEvent : INotification
    {
        public DateTimeOffset CreatedDate { get; protected set; } = DateTimeOffset.UtcNow;
    }
}
