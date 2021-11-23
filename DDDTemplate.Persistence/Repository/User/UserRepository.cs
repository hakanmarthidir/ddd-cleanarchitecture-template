﻿

using DDDTemplate.Domain.AggregatesModel.UserAggregate;
using DDDTemplate.Persistence.Context.Mongo;
using DDDTemplate.Persistence.Repository.Mongo;

namespace DDDTemplate.Persistence.Repository.User
{
    //public class UserRepository : EFRepository<Domain.AggregatesModel.UserAggregate.User>, IUserRepository
    //{
    //    public UserRepository(EFContext dbContext) : base(dbContext)
    //    {

    //    }
    //}

    public class UserRepository : MongoRepository<Domain.AggregatesModel.UserAggregate.User>, IUserRepository
    {
        public UserRepository(IMongoContext dbContext) : base(dbContext)
        {

        }
    }
}
