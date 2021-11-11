using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.Auth.Response
{
    public class IsTokenValidResponse : IDto
    {
        public virtual bool IsValid { get; set; }
    }

}
