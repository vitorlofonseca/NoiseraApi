﻿using System;
using System.Collections.Generic;
using Noisera.Core;

namespace Noisera.Domain
{
    public class Gig : IAggregateRoot
    {
        public string GUID { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string AvatarUrl { get; private set; }
        public string BandGUID { get; private set; }
        public List<Track> Tracks { get; set; }

        public Gig(string guid, string name, string description, string avatarUrl, string bandGUID)
        {
            GUID = String.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() : guid;
            Name = name;
            Description = description;
            AvatarUrl = avatarUrl;
            BandGUID = bandGUID;
        }
    }
}
