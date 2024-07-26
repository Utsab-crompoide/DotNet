using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Saas.Entities.enums;

namespace Saas.Entities
{
    public class SharedSecret : BaseEntity
    {
        public Guid SecretId { get; set; }
        public SharedSecretAccessLevel AccessLevel { get; set; }
        public Guid SharedTo { get; set; }
        public bool HasAllowedEdit { get; set; }
        public Secret? Secret { get; set; }
        public User? User { get; set; }
        public Organization? Organization { get; set; }
    }
}