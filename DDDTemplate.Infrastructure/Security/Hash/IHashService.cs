namespace DDDTemplate.Infrastructure.Security.Hash
{
    public interface IHashService
    {
        string GetHashedString(string value);
        bool VerifyHashes(string actualValue, string hashedValue);
    }
}
