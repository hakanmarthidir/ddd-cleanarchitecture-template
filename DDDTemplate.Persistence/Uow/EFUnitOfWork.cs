using System;
using System.Threading;
using System.Threading.Tasks;
using DDDTemplate.Domain.User;
using DDDTemplate.Persistence.Context.Relational;
using DDDTemplate.Persistence.Repository;
using DDDTemplate.Persistence.Repository.Relational;

namespace DDDTemplate.Persistence.Uow
{
    public class EFUnitOfWork : IEFUnitOfWork
    {
        private readonly EFContext _context;

        private IRepository<User> _userRepository { get; }


        public EFUnitOfWork(EFContext context)
        {
            _context = context;
        }

        public IRepository<User> UserRepository
        {
            get { return _userRepository ?? new EFRepository<User>(_context); }
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
