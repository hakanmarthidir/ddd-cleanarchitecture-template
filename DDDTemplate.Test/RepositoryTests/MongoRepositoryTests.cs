using System;
using System.Collections.Generic;
using System.Linq;
using DDDTemplate.Persistence.Context.Mongo;
using DDDTemplate.Persistence.Repository.User;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDDTemplate.Test.RepositoryTests
{
    [TestClass]
    public class MongoRepositoryTests
    {
        Dictionary<string, string> mockAppSettings = new Dictionary<string, string> {
            {"ConnectionStrings:MongoConnection", "mongodb://localhost:27017/template"}
        };

        // 1. Called once before each test
        public MongoRepositoryTests()
        {
        }

        // 2. Called once before each test after the constructor
        [TestInitialize]
        public void TestInitialize()
        {
            Console.WriteLine("TestInitialize");
        }

        //3.
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


        // 4. Called once after each test before the Dispose method
        [TestCleanup]
        public void TestCleanup()
        {

        }

        // 5. Called once after each test
        public void Dispose()
        {

        }
    }
}
