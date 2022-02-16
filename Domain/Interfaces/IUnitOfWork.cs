using Domain.Entities.UserAggregate;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        int Save();
        Task<int> SaveAsync(CancellationToken token = default(CancellationToken));
    }
}
