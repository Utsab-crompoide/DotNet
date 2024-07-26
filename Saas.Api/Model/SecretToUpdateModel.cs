using System.ComponentModel.DataAnnotations;
using Saas.Entities.enums;

namespace Saas.Api.Model;

public class SecretToUpdateModel
{
    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Content { get; set; }
}