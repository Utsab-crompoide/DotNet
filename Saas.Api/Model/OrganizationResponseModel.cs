using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Saas.Entities;

namespace Saas.Api.Model
{
    public class OrganizationResponseModel
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}