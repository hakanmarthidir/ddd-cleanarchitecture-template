namespace Domain.Shared
{
    public class BaseEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}
