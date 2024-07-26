using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saas.Api.Model
{
    public class RegisterModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Email { get; set; }
    }
}