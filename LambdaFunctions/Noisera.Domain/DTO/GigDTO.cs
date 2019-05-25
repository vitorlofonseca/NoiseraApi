using Noisera.Core;

namespace Noisera.Domain
{
    public class GigDTO : IAggregateRoot
    {
        public string GUID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string AvatarUrl { get; private set; }
        public string BandGUID { get; private set; }

        public GigDTO(Gig gig)
        {
            this.GUID = gig.GUID;
            this.Name = gig.Name;
            this.Description = gig.Description;
            this.AvatarUrl = gig.AvatarUrl;
            this.BandGUID = gig.BandGUID;
        }
    }
}

