using System;

namespace Application.Contracts.Auth.Request
{
    public class UserActivationDto
    {
        public Guid? UserId { get; set; }
        public string ActivationCode { get; set; }
    }
}
