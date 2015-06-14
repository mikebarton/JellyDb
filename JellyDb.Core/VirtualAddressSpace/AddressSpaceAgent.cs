using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace
{
    public class AddressSpaceAgent
    {
        private Guid _addressSpaceId;
        private static object _syncObject = new object();

        internal AddressSpaceAgent(Guid addressSpaceId)
        {
            _addressSpaceId = addressSpaceId;
        }

        public byte[] ReadData(long storageOffset, int numBytesToRead)
        {
            return ReadFromDisk(_addressSpaceId, storageOffset, numBytesToRead);
        }

        public void WriteData(long storageOffset, int bufferIndex, int numBytes, byte[] dataBuffer)
        {
            lock (_syncObject)
            {
                WriteToDisk(_addressSpaceId, storageOffset, bufferIndex, numBytes, dataBuffer);
            }
        }

        public long EndOfFileIndex 
        {
            get { return GetEndOfAddressSpaceOffset(_addressSpaceId); }
        }

        internal event ReadFromAddressSpaceDelegate ReadFromDisk;
        internal event WriteToAddressSpaceDelegate WriteToDisk;
        internal event GetEndOffset GetEndOfAddressSpaceOffset;
    }

    internal delegate void WriteToAddressSpaceDelegate(Guid addressSpaceId, long storageOffset, int bufferIndex, int numBytes, byte[] dataBuffer);
    internal delegate byte[] ReadFromAddressSpaceDelegate(Guid addressSpaceId, long storageOffset, int numBytes);
    internal delegate long GetEndOffset(Guid addressSpaceId);
}
