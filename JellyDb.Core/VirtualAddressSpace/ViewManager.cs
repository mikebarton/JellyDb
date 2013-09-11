using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;

namespace JellyDb.Core.VirtualAddressSpace
{
    public class ViewManager : IViewManager
    {
        private MemoryMappedFile fileMapping;
        private MemoryMappedViewAccessor view;

        public ViewManager(MemoryMappedFile fileMapping)
        {
            this.fileMapping = fileMapping;
            view = fileMapping.CreateViewAccessor();
        }

        public void ReadVirtualPage(ref byte[] dataBuffer, long bufferIndex, long storageOffset, long numBytesToRead)
        {
            //n.b. the intellisense description of parameters is incorrect. 
            //see http://msdn.microsoft.com/es-es/library/dd267761.aspx for correct description
            int result = view.ReadArray<byte>(storageOffset, dataBuffer, (int)bufferIndex, (int)numBytesToRead);
        }

        public void WriteVirtualPage(ref byte[] dataBuffer, long bufferIndex, long storageOffset, long numBytesToWrite)
        {
            //n.b. the intellisense description of parameters is incorrect. 
            //see http://msdn.microsoft.com/es-es/library/dd267754.aspx for correct description
            view.WriteArray<byte>(storageOffset, dataBuffer, (int)bufferIndex, (int)numBytesToWrite);
        }

        public void Flush()
        {

        }

        public void Dispose()
        {
            view.Dispose();
        }
    }
}
