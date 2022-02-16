using System.Threading.Tasks;

namespace Application.Abstraction.Interfaces
{
    public interface IHashService
    {
        string GetHashedString(string hashValue);
        bool VerifyHashes(string actualValue, string hashedValue);
        Task<string> GetHashedStringAsync(string hashValue);
        Task<bool> VerifyHashesAsync(string actualValue, string hashedValue);
    }
}
