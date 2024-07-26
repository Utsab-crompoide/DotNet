using System.ComponentModel.DataAnnotations;
using Saas.Entities.enums;

namespace Saas.Api.Model;

public class OneTimeShareResponseModel
{
    public Guid Id { get; set; }
    public required string Content { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int AccessCount { get; set; }

}