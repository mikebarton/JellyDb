using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace.ViewManagers
{
    public abstract class StreamManager : IViewManager
    {
        protected abstract Stream Stream { get; }

        public void Flush()
        {
            Stream.Flush();
        }

        public void WriteVirtualPage(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToWrite)
        {
            Stream.Position = storageOffset;
            Stream.Write(dataBuffer, bufferIndex, numBytesToWrite);
        }

        public void ReadVirtualPage(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToRead)
        {
            Stream.Position = storageOffset;
            Stream.Read(dataBuffer, bufferIndex, numBytesToRead);
        }

        public void Dispose()
        {
            Flush();
            Stream.Close();
            Stream.Dispose();
        }
    }
}
