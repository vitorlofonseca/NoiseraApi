using Noisera.Core;
using System;

namespace Noisera.Domain
{
    public class TrackDTO : IAggregateRoot
    {
        public string GUID { get; private set; }
        public string Name { get; private set; }
        public string Artist { get; private set; }
        public string SpotifyTrackId { get; private set; }
        public string AvatarUrl { get; private set; }

        public TrackDTO(Track track)
        {
            this.GUID = track.GUID;
            this.Name = track.Name;
            this.Artist = track.Artist;
            this.SpotifyTrackId = track.SpotifyId;
            this.AvatarUrl = track.Image;
        }
    }
}
