using System;

namespace DDDTemplate.Domain.SeedWork
{
    public interface ISoftDelete
    {
        Status Status { get; set; }
        DateTimeOffset? DeletedDate { get; set; }
    }
}
