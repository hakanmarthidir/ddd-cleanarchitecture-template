namespace Domain.Shared
{
    public abstract class BaseEntity
    {        

        public List<DomainEvent> _domainEvents = new List<DomainEvent>();

        public void RaiseEvent(DomainEvent domainEvent)
        {
            this._domainEvents.Add(domainEvent);
        }

        public void ClearEvents()
        {
            this._domainEvents.Clear();
        }
    }
}
