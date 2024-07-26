using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saas.Api.Model
{
    public class LoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}