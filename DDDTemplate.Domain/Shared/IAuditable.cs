using System;
namespace DDDTemplate.Domain.Shared
{
    public interface IAuditable
    {
        DateTimeOffset? CreatedDate { get; set; }
        DateTimeOffset? ModifiedDate { get; set; }
    }
}
