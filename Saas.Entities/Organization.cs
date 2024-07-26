using Saas.Entities;

namespace Entities;

public class Organization : BaseEntity
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Guid? PaymentPlanId { get; set; }
    public bool IsPaid { get; set; }
    public DateTime? PlanExpiryDate { get; set; }
    public PaymentPlan? PaymentPlan { get; set; }
    public ICollection<OrganizationUser>? OrganizationUsers { get; set; }
    public ICollection<SharedSecret>? SharedSecrets { get; set; }
    public ICollection<Collection>? SecretCollections { get; set; }
}