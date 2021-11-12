using System;
using System.Collections.Generic;
using System.Linq;
using DDDTemplate.Infrastructure.Security.Hash;
using DDDTemplate.Persistence.Context.Mongo;
using DDDTemplate.Persistence.Repository.User;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Moq;

namespace DDDTemplate.Test.RepositoryTests
{
    [TestClass]
    public class MongoRepositoryTests
    {
        Dictionary<string, string> mockAppSettings = new Dictionary<string, string> {
            {"ConnectionStrings:MongoConnection", "mongodb://localhost:27017/template"}
        };

        [TestMethod]
        public void MongoRepository_Insert_ShouldBeTrue()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(mockAppSettings)
                .Build();

            var _context = new MongoContext(configuration);
            UserRepository _repository = new UserRepository(_context);

            _repository.Insert(new Domain.AggregatesModel.UserAggregate.User()
            {
                FirstName = "xyz",
                ModifiedDate = DateTime.UtcNow,
                LastName = "def",
                CreatedDate = DateTime.UtcNow,
                Email = "aaaa@abcd.com",
                IsActivated = Domain.AggregatesModel.UserAggregate.Enums.ActivationStatus.NotActivated,
                Status = Domain.SeedWork.Status.Active,
                UserType = Domain.AggregatesModel.UserAggregate.Enums.UserType.User
            });

            var users = _repository.Find(x => x.Email == "aaaa@abcd.com").ToList();
            Assert.IsTrue(users.Count > 0);
        }
    }
}
