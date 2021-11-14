using DDDTemplate.Domain.Enums;
using System;

namespace DDDTemplate.Domain.Interfaces
{
    public interface ISoftDelete
    {
        Status Status { get; set; }
        DateTimeOffset? DeletedDate { get; set; }
    }
}
