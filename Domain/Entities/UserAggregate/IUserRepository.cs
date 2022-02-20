using Domain.Interfaces;

namespace Domain.Entities.UserAggregate
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
