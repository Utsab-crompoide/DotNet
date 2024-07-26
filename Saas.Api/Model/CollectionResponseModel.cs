using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Saas.Entities.enums;

namespace Saas.Api.Model
{
    public class CollectionResponseModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Color { get; set; }
        public SecretCollectionType? Type { get; set; }
        public Guid? OrganizationId { get; set; }

    }
}