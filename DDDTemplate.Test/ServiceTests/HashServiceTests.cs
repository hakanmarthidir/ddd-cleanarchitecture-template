using System;
using DDDTemplate.Infrastructure.Security.Hash;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DDDTemplate.Test.ServiceTests
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
        public void HashString_Creation_Test()
        {
            string source = "hello";
            var hashed = this.hashService.GetHashedString(source);
            Assert.IsNotNull(hashed);
        }

        [TestMethod]
        public void HashString_Verify_Test()
        {
            string source = "hello";
            var hashed = this.hashService.GetHashedString(source);
            var isVerified = this.hashService.VerifyHashes(source, hashed);
            Assert.IsTrue(isVerified);
        }

        [TestMethod]
        public void HashString_Creation_Exception_Test()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                return this.hashService.GetHashedString("");
            });
        }

    }
}
