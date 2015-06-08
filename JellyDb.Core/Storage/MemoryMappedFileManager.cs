using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;

namespace JellyDb.Core.VirtualAddressSpace.Storage
{
    public class MemryMappedFileManager : IDataStorage
    {
        private MemoryMappedFile fileMapping;
        private MemoryMappedViewAccessor view;

        public MemryMappedFileManager(MemoryMappedFile fileMapping)
        {
            this.fileMapping = fileMapping;
            view = fileMapping.CreateViewAccessor();
        }

        public void ReadData(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToRead)
        {
            //n.b. the intellisense description of parameters is incorrect. 
            //see http://msdn.microsoft.com/es-es/library/dd267761.aspx for correct description
            int result = view.ReadArray<byte>(storageOffset, dataBuffer, (int)bufferIndex, (int)numBytesToRead);
        }

        public void WriteData(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToWrite)
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




        public void Initialise()
        {
        }


        public void Initialise(System.IO.Stream stream)
        {
            throw new NotImplementedException();
        }


        public byte[] ReadToEnd(long storageOffset)
        {
            throw new NotImplementedException();
        }
    }
}
