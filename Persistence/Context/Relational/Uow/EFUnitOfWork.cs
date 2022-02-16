using Domain.Entities.UserAggregate;
using Domain.Interfaces;
using Persistence.Repository.User;

namespace Persistence.Context.Relational.Uow
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly EFContext _context;

        public EFUnitOfWork(EFContext context)
        {
            _context = context;
        }

        private IUserRepository _userRepository { get; }
        public IUserRepository UserRepository
        {
            get { return _userRepository ?? new UserRepository(_context); }
        }

        public int Save()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public async Task<int> SaveAsync(CancellationToken token = default(CancellationToken))
        {
            return await _context.SaveChangesAsync(token).ConfigureAwait(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }


    }
}
