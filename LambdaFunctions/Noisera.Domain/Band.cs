using System;
using Noisera.Core;

namespace Noisera.Domain
{
    public class Band : IAggregateRoot
    {
        public string GUID { get; private set; }
        public string Name { get; private set; }

        public Band(string guid, string name)
        {
            GUID = String.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() : guid;
            Name = name;
        }
    }
}
