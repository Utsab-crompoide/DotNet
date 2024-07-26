using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saas.Entities.Dtos
{
    public class Result(bool Success, string Message)
    {
        public bool Success { get; } = Success;
        public string Message { get; } = Message;
    }
}