using System;
namespace DDDTemplate.Domain.SeedWork
{
    public interface IAuditable
    {
        DateTimeOffset? CreatedDate { get; set; }
        DateTimeOffset? ModifiedDate { get; set; }
    }
}
