﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Auth.Response
{
    public class GetRegisteredUserEmailDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
