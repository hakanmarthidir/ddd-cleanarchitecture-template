using System;
using Microsoft.Extensions.Configuration;

namespace DDDTemplate.Infrastructure.Security.Token
{
    public interface ITokenService
    {
        string GenerateToken(Guid Id);
        bool ValidateCurrentToken(string token);

    }
}
