using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace.Storage
{
    public interface IDataStorage : IDisposable
    {
        void Flush();
        void WriteVirtualPage(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToWrite);
        void ReadVirtualPage(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToRead);
        void Initialise(string databaseName);
    }
}
