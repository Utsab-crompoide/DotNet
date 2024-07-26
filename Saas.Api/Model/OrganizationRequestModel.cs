using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Saas.Entities;

namespace Saas.Api.Model
{
    public class OrganizationRequestModel
    {
        [Required]
        [MaxLength(100)]
        public required string Name { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
    }
}