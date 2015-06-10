using JellyDb.Core.Engine.Fun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Hosting
{
    public class AddressSpaceIndex : DataWritableBase
    {
        public AddressSpaceIndex()
        {
            Pairs = new List<IndexDataPair>();
        }

        public static Guid IndexRootId = new Guid(new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f });

        public List<IndexDataPair> Pairs { get; set; }
    }

    public struct IndexDataPair
    {
        public Guid IndexId { get; set; }
        public Guid DataId { get; set; }
    }
}
