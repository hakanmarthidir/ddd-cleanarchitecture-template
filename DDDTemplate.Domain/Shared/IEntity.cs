using System;

namespace DDDTemplate.Domain.Shared
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
