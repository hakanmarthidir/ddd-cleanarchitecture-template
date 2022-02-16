using System;
using System.IdentityModel.Tokens.Jwt;

namespace Application.Abstraction.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(Guid Id);
        bool ValidateCurrentToken(string token);
        Guid? GetUserIdByToken(string token);
        JwtSecurityToken GetToken(string token);
    }
}
