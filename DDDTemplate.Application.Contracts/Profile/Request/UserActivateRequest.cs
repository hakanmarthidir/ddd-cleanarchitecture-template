using System;
using System.ComponentModel.DataAnnotations;
using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.Profile.Request
{
    public class UserActivateRequest : IDto
    {
        [Required]
        public virtual Guid UserId { get; set; }
        [Required]
        public virtual string ActivationCode { get; set; }
    }


}
