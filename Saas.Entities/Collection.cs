using Saas.Entities;
using Saas.Entities.enums;

namespace Entities;

public class Collection : BaseEntity
{
    public required string Name { get; set; }
    public string? Color { get; set; }
    public bool IsShared { get; set; }
    public SecretCollectionType? Type { get; set; }
    public Guid? OrganizationId { get; set; }
    public Guid? UserId { get; set; }
    public Organization? Organization { get; set; }
    public User? User { get; set; }

    public ICollection<CollectionSecret> CollectionSecrets { get; set; } = [];
}