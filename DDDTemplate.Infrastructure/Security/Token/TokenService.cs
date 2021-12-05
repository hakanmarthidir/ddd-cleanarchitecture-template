using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DDDTemplate.Application.Abstraction.External;
using DDDTemplate.Infrastructure.Security.Token.Config;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DDDTemplate.Infrastructure.Security.Token
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfig _jwtConfig;
        
        public TokenService(IOptionsMonitor<JwtConfig> jwtConfig)
        {
            this._jwtConfig = jwtConfig.CurrentValue;
        }

        public string GenerateToken(Guid userId)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(this._jwtConfig.Duration.GetValueOrDefault(7)),
                Issuer = this._jwtConfig.Issuer,
                Audience = this._jwtConfig.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        private JwtSecurityToken GetToken(string token)
        {
            try
            {
                var mySecret = _jwtConfig.Secret;
                var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
                var myIssuer = this._jwtConfig.Issuer;
                var myAudience = this._jwtConfig.Audience;
                var tokenHandler = new JwtSecurityTokenHandler();

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = mySecurityKey,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                return jwtToken;

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }


        public bool ValidateCurrentToken(string token)
        {
            var validatedToken = this.GetToken(token);
            return validatedToken != null;
        }

        public Guid? GetUserIdByToken(string token)
        {
            var validatedToken = this.GetToken(token);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            return userId;
        }
    }
}
