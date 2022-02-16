using Domain.Entities.UserAggregate;
using Persistence.Context.Relational;
using Persistence.Repository.Relational;

namespace Persistence.Repository.User
{
    public class UserRepository : EFRepository<Domain.Entities.UserAggregate.User>, IUserRepository
    {
        public UserRepository(EFContext dbContext) : base(dbContext)
        {

        }
    }
}
