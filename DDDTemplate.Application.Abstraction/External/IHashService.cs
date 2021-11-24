using System.Threading.Tasks;

namespace DDDTemplate.Application.Abstraction.External
{
    public interface IHashService
    {
        string GetHashedString(string hashValue);
        bool VerifyHashes(string actualValue, string hashedValue);
        Task<string> GetHashedStringAsync(string hashValue);
        Task<bool> VerifyHashesAsync(string actualValue, string hashedValue);
    }
}
