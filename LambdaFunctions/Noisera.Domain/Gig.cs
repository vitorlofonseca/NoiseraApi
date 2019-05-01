using System;
using Noisera.Core;

namespace Noisera.Domain
{
    public class Gig : IAggregateRoot
    {
        public string GUID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string AvatarUrl { get; private set; }
        public int? SpotifyPlaylistId { get; private set; }
        public string BandGUID { get; private set; }

        public Gig(string guid, string name, string description, string avatarUrl, int? spotifyPlaylistId, string bandGUID)
        {
            GUID = String.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() : guid;
            Name = name;
            Description = description;
            AvatarUrl = avatarUrl;
            SpotifyPlaylistId = spotifyPlaylistId;
            BandGUID = bandGUID;
        }
    }
}
