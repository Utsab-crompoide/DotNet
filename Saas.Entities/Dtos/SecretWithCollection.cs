using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace Saas.Entities.Dtos
{
    public class SecretWithCollection
    {
        public required Secret Secret { get; set; }
        public List<Guid>? CollectionIds { get; set; }
    }
}