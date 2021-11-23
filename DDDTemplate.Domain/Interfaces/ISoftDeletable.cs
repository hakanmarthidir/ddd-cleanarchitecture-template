using DDDTemplate.Domain.Enums;
using System;

namespace DDDTemplate.Domain.Interfaces
{
    public interface ISoftDeletable
    {
        Status Status { get; set; }
        DateTimeOffset? DeletedDate { get; set; }
        string DeletedBy { get; set; }
    }
}
