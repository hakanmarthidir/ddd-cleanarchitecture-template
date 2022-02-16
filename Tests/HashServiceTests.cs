using System;
using Infrastructure.Security.Hash;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.ServiceTests
{
    [TestClass]
    public class HashServiceTests
    {

        HashService hashService;

        [TestInitialize]
        public void Initialize()
        {
            this.hashService = new HashService();
        }


        [TestMethod]
        public async System.Threading.Tasks.Task HashString_Creation_TestAsync()
        {
            string source = "hello";
            var hashed = await this.hashService.GetHashedStringAsync(source).ConfigureAwait(false);
            Assert.IsNotNull(hashed);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task HashString_Verify_TestAsync()
        {
            string source = "hello";
            var hashed = await this.hashService.GetHashedStringAsync(source).ConfigureAwait(false);
            var isVerified = await this.hashService.VerifyHashesAsync(source, hashed).ConfigureAwait(false);
            Assert.IsTrue(isVerified);
        }

        [TestMethod]
        public async System.Threading.Tasks.Task HashString_Creation_Exception_TestAsync()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
            {
                await this.hashService.GetHashedStringAsync("").ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

    }
}
