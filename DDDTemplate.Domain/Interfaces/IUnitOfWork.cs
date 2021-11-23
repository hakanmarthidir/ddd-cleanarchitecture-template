using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDDTemplate.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IUserRepository UserRepository { get; }
        int Save();
        Task<int> SaveAsync(CancellationToken token = default(CancellationToken));
    }
}
