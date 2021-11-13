using System;
using System.Threading;
using System.Threading.Tasks;
using DDDTemplate.Domain.AggregatesModel.UserAggregate;
using DDDTemplate.Persistence.Repository.User;

namespace DDDTemplate.Persistence.Context.Relational.Uow
{
    public class EFUnitOfWork : IEFUnitOfWork
    {
        private readonly EFContext _context;

        public EFUnitOfWork(EFContext context)
        {
            _context = context;
        }

        //private IUserRepository _userRepository { get; }
        //public IUserRepository UserRepository
        //{
        //    get { return _userRepository ?? new UserRepository(_context); }
        //}

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
