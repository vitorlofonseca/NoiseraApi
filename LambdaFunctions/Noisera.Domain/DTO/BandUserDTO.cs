using Noisera.Core;

namespace Noisera.Domain
{
    public class BandUserDTO : IAggregateRoot
    {
        public string SpotifyUserId { get; private set; }
        public string BandGUID { get; private set; }

        public BandUserDTO(string spotifyUserId, string bandGUID)
        {
            SpotifyUserId = spotifyUserId;
            BandGUID = bandGUID;
        }
    }
}

