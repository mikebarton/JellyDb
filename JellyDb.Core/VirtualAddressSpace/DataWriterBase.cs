using JellyDb.Core.VirtualAddressSpace.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace
{
    public class DataWriterBase : IDisposable
    {
        protected IDataStorage _dataStorage;

        public DataWriterBase(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public void Flush()
        {
            _dataStorage.Flush();
        }

        protected byte[] ReadFromDisk(long storageOffset, int numBytesToRead)
        {
            var retrievedPage = new byte[numBytesToRead];
            _dataStorage.ReadData(ref retrievedPage, 0, storageOffset, numBytesToRead);
            return retrievedPage;
        }

        protected long WriteToDisk(byte[] dataBuffer)
        {
            var endOfFile = _dataStorage.EndOfFileIndex;
            _dataStorage.WriteData(ref dataBuffer, 0, endOfFile, dataBuffer.Length);
            return endOfFile;
        }

        public void Dispose()
        {
            _dataStorage.Dispose();
        }
    }

}
