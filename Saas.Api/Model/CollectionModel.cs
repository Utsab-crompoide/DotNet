using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Saas.Entities.enums;

namespace Saas.Api.Model
{
    public class CollectionModel
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        public SecretCollectionType? Type { get; set; }
        public Guid? OrganizationId { get; set; }

    }
}