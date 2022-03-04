using Domain.Entities.UserAggregate;
using Domain.Entities.UserAggregate.Enums;
using System;

namespace Application.Contracts.Auth.Response
{
    public class UserLoggedinDto
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Token { get; set; }
        public virtual UserTypeEnum UserType { get; set; }
        public virtual UserLoggedinActivationDto Activation { get; set; }
    }
}
