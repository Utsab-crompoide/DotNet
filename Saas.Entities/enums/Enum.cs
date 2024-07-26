namespace Saas.Entities.enums
{
    public enum SecretCollectionType
    {
        user,
        organization
    }
    public enum SharedSecretAccessLevel
    {
        organization,
        private_share
    }
    public enum OrganizationUserRole
    {
        owner,
        admin,
        staff
    }
    public enum SecretType
    {
        password,
        secure_notes,
        bank_accounts,
        payment_card,
        addresses,
        plain_text,
        environment_variables
    }
    public enum UserCognitoGroup
    {
        SUPER_ADMIN,
        USER
    }
    public enum PaymentPlanType
    {
        individual,
        organization
    }
    public enum PaymentMode
    {
        paypal,
        bank_provider
    }

    public enum PaymentStatus
    {
        initiated,
        pending_verification,
        gateway_timeout,
        verified,
        rejected
    }
}