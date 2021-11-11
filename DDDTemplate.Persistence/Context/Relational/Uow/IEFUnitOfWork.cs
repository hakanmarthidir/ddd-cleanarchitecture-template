using System;
using System.Threading;
using System.Threading.Tasks;
using DDDTemplate.Domain.AggregatesModel.UserAggregate;

namespace DDDTemplate.Persistence.Context.Relational.Uow
{
    public interface IEFUnitOfWork : IDisposable
    {
        //IUserRepository UserRepository { get; }
        int Save();
        Task<int> SaveAsync(CancellationToken token = default(CancellationToken));
    }
}
