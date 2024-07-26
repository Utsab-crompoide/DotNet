using Entities;

namespace Saas.Entities
{
    public class OneTimeShare : BaseEntity
    {
        public required string Content { get; set; }
        public required string IV { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int AccessCount { get; set; }
    }
}