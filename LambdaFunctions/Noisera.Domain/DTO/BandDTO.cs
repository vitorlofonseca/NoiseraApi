using Noisera.Core;

namespace Noisera.Domain
{
    public class BandDTO : IAggregateRoot
    {
        public string GUID { get; private set; }
        public string Name { get; private set; }

        public BandDTO(Band band)
        {
            this.GUID = band.GUID;
            this.Name = band.Name;
        }
    }
}

