using System;
namespace DDDTemplate.Domain.Shared
{
    public abstract class BaseEntity
    {
        public virtual Guid Id { get; protected set; }
    }
}
