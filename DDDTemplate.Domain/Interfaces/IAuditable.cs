using System;
namespace DDDTemplate.Domain.Interfaces
{
    public interface IAuditable
    {
        DateTimeOffset? CreatedDate { get; set; }
        DateTimeOffset? ModifiedDate { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy { get; set; }
    }
}
