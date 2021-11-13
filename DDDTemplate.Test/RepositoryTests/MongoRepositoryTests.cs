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
        public async System.Threading.Tasks.Task MongoRepository_Insert_ShouldBeTrueAsync()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(mockAppSettings)
                .Build();

            var _context = new MongoContext(configuration);
            UserRepository _repository = new UserRepository(_context);

            await _repository.InsertAsync(new Domain.AggregatesModel.UserAggregate.User()
            {
                FirstName = "xyz",
                ModifiedDate = DateTimeOffset.UtcNow,
                LastName = "def",
                CreatedDate = DateTimeOffset.UtcNow,
                Email = "aaaa@abcd.com",
                IsActivated = Domain.AggregatesModel.UserAggregate.Enums.ActivationStatus.NotActivated,
                Status = Domain.SeedWork.Status.Active,
                UserType = Domain.AggregatesModel.UserAggregate.Enums.UserType.User
            }).ConfigureAwait(false);

            var users = _repository.Find(x => x.Email == "aaaa@abcd.com").ToList();
            Assert.IsTrue(users.Count > 0);


            var userFirstOrDefault = await _repository.FirstOrDefaultAsync(x => x.Email == "aaaa@abcd.com").ConfigureAwait(false);
            Assert.IsTrue(userFirstOrDefault != null);

            var user = await _repository.FindByIdAsync(userFirstOrDefault.Id).ConfigureAwait(false);
            Assert.IsTrue(user != null);

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
