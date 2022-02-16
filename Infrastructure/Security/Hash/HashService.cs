using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Application.Abstraction.Interfaces;

namespace Infrastructure.Security.Hash
{
    public class HashService : IHashService
    {
        public string GetHashedString(string hashValue)
        {
            Guard.Against.NullOrEmpty(hashValue, "Hash value can not be null.");

            string hashedValue = string.Empty;

            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(hashValue);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                hashedValue = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }

            return hashedValue;
        }

        public async Task<string> GetHashedStringAsync(string hashValue)
        {
            var myHashFunc = new Func<string, string>((hash) =>
            {
                return this.GetHashedString(hash);
            });

            return await Task.Run<string>(() =>
            {
                return myHashFunc(hashValue);
            }).ConfigureAwait(false);
        }


        public bool VerifyHashes(string actualValue, string hashedValue)
            => GetHashedString(actualValue) == hashedValue;

        public async Task<bool> VerifyHashesAsync(string actualValue, string hashedValue)
             => await GetHashedStringAsync(actualValue) == hashedValue;

    }
}
