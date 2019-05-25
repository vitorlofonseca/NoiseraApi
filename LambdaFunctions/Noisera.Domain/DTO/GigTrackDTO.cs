using Noisera.Core;

namespace Noisera.Domain
{
    public class GigTrackDTO : IAggregateRoot
    {
        public bool Active { get; private set; }
        public int Order { get; private set; }
        public string GigGUID { get; private set; }
        public string TrackGUID { get; private set; }

        public GigTrackDTO(Track track, Gig gig)
        {
            this.Active = track.Active;
            this.Order = track.Order;
            this.GigGUID = gig.GUID;
            this.TrackGUID = track.GUID;
        }
    }
}
