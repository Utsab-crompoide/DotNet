using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saas.Api.Model
{
    public class ConfirmModel
    {
        public required string Username { get; set; }
        public required string ConfirmationCode { get; set; }
    }
}