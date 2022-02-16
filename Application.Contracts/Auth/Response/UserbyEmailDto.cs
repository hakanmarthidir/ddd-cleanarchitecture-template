using System;

namespace Application.Contracts.Auth.Response
{
    public class UserbyEmailDto
    {
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
