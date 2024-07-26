using Saas.Entities;
using Saas.Entities.enums;

namespace Entities;

public class OrganizationUser : BaseEntity
{
    public Guid CognitoUserId { get; set; }
    public Guid OrganizationId { get; set; }
    public OrganizationUserRole Role { get; set; }
    public DateTime? InvitationAcceptedAt { get; set; }
    public Guid? InvitingUserId { get; set; }
    public Organization? Organization { get; set; }
    public User? CognitoUser { get; set; }
}