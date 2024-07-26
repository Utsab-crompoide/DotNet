using Saas.Entities;
using Saas.Entities.enums;

namespace Entities;

public class Secret : BaseEntity
{
    public required string Name { get; set; }
    public SecretType Type { get; set; }
    public required string Content { get; set; }
    public bool IsShared { get; set; }
    public User? CreatingUser { get; set; }
    public ICollection<SharedSecret> SharedSecrets { get; set; } = [];
    public ICollection<CollectionSecret> CollectionSecrets { get; set; } = [];
}