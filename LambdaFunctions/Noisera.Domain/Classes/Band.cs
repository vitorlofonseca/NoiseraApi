using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Noisera.Core;

namespace Noisera.Domain
{
    public class Band : IAggregateRoot
    {
        public string GUID { get; private set; }
        public string Name { get; private set; }
        public List<string> SpotifyUsersId { get; private set; }

        public Band(string guid, string name, List<string> spotifyUsersId)
        {
            GUID = String.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() : guid;
            Name = name;
            SpotifyUsersId = spotifyUsersId;
        }
    }
}
