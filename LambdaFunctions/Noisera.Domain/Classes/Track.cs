using Noisera.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Noisera.Domain
{
    public class Track : IAggregateRoot
    {
        public string GUID { get; set; }
        public bool Active { get; private set; }
        public string Album { get; private set; }
        public string Artist { get; private set; }
        public string Image { get; private set; }
        public string Name { get; private set; }
        public int Order { get; private set; }
        public string SpotifyId { get; private set; }
        public int Year { get; private set; }
        public string Observations { get; private set; }

        public Track(
            string guid, 
            bool active, 
            string album, 
            string artist, 
            string image, 
            string name, 
            int order, 
            string spotifyId, 
            int year,
            string observations)
        {
            this.GUID = String.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() : guid;
            this.Active = active;
            this.Album = album;
            this.Artist = artist;
            this.Image = image;
            this.Name = name;
            this.Order = order;
            this.SpotifyId = spotifyId;
            this.Year = year;
            this.Observations = observations;
        }
    }
}
