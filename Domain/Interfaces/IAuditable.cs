namespace Domain.Interfaces
{
    public interface IAuditable
    {
        DateTimeOffset? CreatedDate { get; }
        DateTimeOffset? ModifiedDate { get;  }
        string CreatedBy { get;  }
        string ModifiedBy { get; }
    }
}
