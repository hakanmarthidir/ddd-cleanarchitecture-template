using Domain.Enums;

namespace Domain.Interfaces
{
    public interface ISoftDeletable
    {
        Status Status { get; }
        DateTimeOffset? DeletedDate { get; }
        string DeletedBy { get; }

        void SoftDelete(string deletedBy = "Admin");
    }
}
