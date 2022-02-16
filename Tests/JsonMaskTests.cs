using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;

namespace MsTests
{
    [TestClass]
    public class JsonMaskTests
    {
        [TestMethod]
        public void JsonMask_Masked_Correctly_Test()
        {

            var example = new
            {
                Email = "hakan@abcd.com", SomeValue = "Demo", Password = "SomePasswordHere",
                Depth = new { Password = "!§$%&/()=?'*,*/-+!§$§$%$%&//()=(?", A = new {Number = "123123123123", Email = "hakan@abcd.com"}},
                CreditCard = "11111111111111",
                V = new { D = new {Trial = new { Password = "1234567890!!#", X = new { Email = "hakan@abcd.com", Password = "asmdhjagsdjh" } }}
                }
            };

            var sample = JsonConvert.SerializeObject(example);
            var maskedValue = JsonMask(sample);

            Console.WriteLine(maskedValue);
            Assert.IsNotNull(maskedValue);

        }

        public string JsonMask(string data)
        {
            var blacklist = new string[] { "Password", "Email" };
            var mask = "******";
            var maskedValue = data;

            foreach (var item in blacklist)
            {
                var pattern = $"(?<=\"{item}\":\")[^\"]+(?=\")";
                maskedValue = Regex.Replace(maskedValue, pattern, mask);
            }
            return maskedValue;
        }
    }
}
