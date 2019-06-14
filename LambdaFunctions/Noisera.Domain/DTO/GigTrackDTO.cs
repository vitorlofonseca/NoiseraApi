using Noisera.Core;

namespace Noisera.Domain
{
    public class GigTrackDTO : IAggregateRoot
    {
        public bool Active { get; private set; }
        public int Order { get; private set; }
        public string GigGUID { get; private set; }
        public string TrackGUID { get; private set; }
        public string Observations { get; private set; }

        public GigTrackDTO(Track track, Gig gig)
        {
            this.Active = track.Active;
            this.Order = track.Order;
            this.GigGUID = gig.GUID;
            this.TrackGUID = track.GUID;
        }

        public GigTrackDTO(bool Active, int Order, string GigGUID, string TrackGUID, string Observations)
        {
            this.Active = Active;
            this.Order = Order;
            this.GigGUID = GigGUID;
            this.TrackGUID = TrackGUID;
            this.Observations = Observations;
        }
    }
}
