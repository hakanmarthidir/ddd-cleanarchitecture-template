using System;
using DDDTemplate.Application.Contracts.Shared;

namespace DDDTemplate.Application.Contracts.Profile.Response
{
    public class UserProfileDto
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual ActivationState IsActivated { get; set; }
    }
}
