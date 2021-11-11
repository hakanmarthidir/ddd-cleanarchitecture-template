using System;
using System.Security.Cryptography;
using System.Text;
using DDDTemplate.Core.Guard;

namespace DDDTemplate.Infrastructure.Security.Hash
{
    public class HashService : IHashService
    {
        public string GetHashedString(string hashValue)
        {
            GuardClauses.ArgumentNotNullOrWhitespace(hashValue, nameof(hashValue));

            string hashedValue = string.Empty;

            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(hashValue);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                hashedValue = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }

            return hashedValue;
        }

        public bool VerifyHashes(string actualValue, string hashedValue)
             => GetHashedString(actualValue) == hashedValue;

    }
}
