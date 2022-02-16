using System;

namespace Application.Contracts.Auth.Response
{
    public class JwtMiddlewareDto
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
    }

}
