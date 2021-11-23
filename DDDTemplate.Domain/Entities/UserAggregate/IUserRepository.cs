using System;
using DDDTemplate.Domain.Interfaces;

namespace DDDTemplate.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IRepository<User>
    {
    }
}
