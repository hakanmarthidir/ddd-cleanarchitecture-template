using System;
using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.Profile.Request
{
    public class UserActivateDto
    {
        public virtual Guid UserId { get; set; }
        public virtual string ActivationCode { get; set; }
    }

}
