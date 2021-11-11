using System;

namespace DDDTemplate.Application.Contracts.Auth.Response
{
    public class UserbyEmailResponse
    {
        public string Email { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}
