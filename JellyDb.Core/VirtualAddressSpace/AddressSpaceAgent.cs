using JellyDb.Core.VirtualAddressSpace.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace
{
    public class AddressSpaceAgent : IDataStorage
    {
        private Guid _addressSpaceId;
        private static object _syncObject = new object();

        internal AddressSpaceAgent(Guid addressSpaceId)
        {
            _addressSpaceId = addressSpaceId;
        }

        public void WriteData(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToWrite)
        {
            lock (_syncObject)
            {
                WriteToDisk(_addressSpaceId, storageOffset, bufferIndex, numBytesToWrite, dataBuffer);
            }
        }

        public void ReadData(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToRead)
        {
            var retrievedData = ReadFromDisk(_addressSpaceId, storageOffset, numBytesToRead);
            retrievedData.CopyTo(dataBuffer, bufferIndex);
        }

        public byte[] ReadToEndOfAddressSpace(long storageOffset)
        {
            return ReadToEnd(_addressSpaceId, storageOffset);
        }

        public void Initialise() { }

        public void Dispose()
        {
            Flush();
        }
        
        public long EndOfFileIndex 
        {
            get { return GetEndOfAddressSpaceOffset(_addressSpaceId); }
        }

        internal event ReadFromAddressSpaceDelegate ReadFromDisk; 
        internal event WriteToAddressSpaceDelegate WriteToDisk;
        internal event GetEndOffsetDelegate GetEndOfAddressSpaceOffset;
        internal event ReadToEndDelegate ReadToEnd;
        internal event FlushToDiskDelegate FlushToDisk;

        public void Flush()
        {
            FlushToDisk(_addressSpaceId);
        }
    }

    internal delegate void WriteToAddressSpaceDelegate(Guid addressSpaceId, long storageOffset, int bufferIndex, int numBytes, byte[] dataBuffer);
    internal delegate byte[] ReadFromAddressSpaceDelegate(Guid addressSpaceId, long storageOffset, int numBytes);
    internal delegate long GetEndOffsetDelegate(Guid addressSpaceId);
    internal delegate byte[] ReadToEndDelegate(Guid addressSpceId,  long storageOffset);
    internal delegate void FlushToDiskDelegate(Guid addressSpaceId);
}
