using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.Auth.Response
{
    public class TokenValidationDto
    {
        public virtual bool IsValid { get; set; }
    }

}
