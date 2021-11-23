using System;
using DDDTemplate.Domain.Interfaces;

namespace DDDTemplate.Domain.Entities.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
