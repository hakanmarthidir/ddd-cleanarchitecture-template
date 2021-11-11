using System;

namespace DDDTemplate.Domain.Shared
{
    public interface ISoftDelete
    {
        Status Status { get; set; }
        DateTimeOffset? DeletedDate { get; set; }
    }
}
