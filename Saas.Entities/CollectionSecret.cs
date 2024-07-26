using Entities;

namespace Saas.Entities
{
    public class CollectionSecret
    {
        public Guid CollectionId { get; set; }
        public Guid SecretId { get; set; }

        public Collection? Collection { get; set; }
        public Secret? Secret { get; set; }

    }
}