using System;

namespace DDDTemplate.Application.Abstraction.External
{
    public interface ITokenService
    {
        string GenerateToken(Guid Id);
        bool ValidateCurrentToken(string token);
        Guid? GetUserIdByToken(string token);
    }
}
