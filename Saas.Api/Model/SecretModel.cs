using System.ComponentModel.DataAnnotations;
using Saas.Entities.enums;

namespace Saas.Api.Model;

public class SecretModel
{
    [Required]
    [MaxLength(100)]
    public required string Name { get; set; }
    public SecretType Type { get; set; }
    public required string Content { get; set; }
    public bool IsShared { get; set; }

}