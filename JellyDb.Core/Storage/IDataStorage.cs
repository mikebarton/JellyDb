using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace.Storage
{
    public interface IDataStorage : IDisposable
    {
        void Flush();
        long EndOfFileIndex { get;}
        void WriteData(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToWrite);
        void ReadData(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToRead);
        byte[] ReadToEndOfAddressSpace(long storageOffset);
        void Initialise();
    }
}
