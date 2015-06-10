using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace
{
    public class AddressSpaceAgent : DataWriterBase
    {
        private Guid _addressSpaceId;
        private AddressSpaceManager _manager;

        public AddressSpaceAgent(Guid addressSpaceId, AddressSpaceManager manager)
        {
            _addressSpaceId = addressSpaceId;
            _manager = manager;
        }
    }
}
