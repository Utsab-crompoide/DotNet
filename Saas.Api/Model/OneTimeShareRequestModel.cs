using System.ComponentModel.DataAnnotations;

namespace Saas.Api.Model;

public class OneTimeShareRequestModel
{
    [Required]
    public required string Content { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int AccessCount { get; set; }
    public required string Salt { get; set; }
    public required string IV { get; set; }

}