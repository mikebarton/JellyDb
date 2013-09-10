using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kenny.DbEngine.Core.VirtualAddressSpace
{
    public interface IViewManager : IDisposable
    {
        void Flush();
        void WriteVirtualPage(ref byte[] dataBuffer, long bufferIndex, long storageOffset, long numBytesToWrite);
        void ReadVirtualPage(ref byte[] dataBuffer, long bufferIndex, long storageOffset, long numBytesToRead);
    }
}
