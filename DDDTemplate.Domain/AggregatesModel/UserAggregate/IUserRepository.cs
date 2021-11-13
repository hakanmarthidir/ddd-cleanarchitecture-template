using System;
using DDDTemplate.Domain.SeedWork;

namespace DDDTemplate.Domain.AggregatesModel.UserAggregate
{
    public interface IUserRepository : IMongoRepository<User>
    {
    }
}
