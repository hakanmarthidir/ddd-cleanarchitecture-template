using System;
using System.Threading;
using System.Threading.Tasks;
using DDDTemplate.Domain.User;
using DDDTemplate.Persistence.Repository;

namespace DDDTemplate.Persistence.Uow
{
    public interface IEFUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        int Save();
        Task<int> SaveAsync(CancellationToken token = default(CancellationToken));
    }
}
