﻿using System;

namespace DDDTemplate.Application.Contracts.Auth.Response
{
    public class UserLoggedinDto
    {
        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string Token { get; set; }
        public virtual int UserType { get; set; }
        public virtual int IsActivated { get; set; }
    }

}
