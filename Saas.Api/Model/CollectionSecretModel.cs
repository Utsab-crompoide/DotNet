namespace Saas.Api.Model
{
    public class CollectionSecretModel
    {
        public Guid CollectionId { get; set; }
        public required string Name { get; set; }
    }
}