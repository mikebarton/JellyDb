using JellyDb.Core.VirtualAddressSpace.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace
{
    public class DataWriterBase
    {
        private IDataStorage _dataStorage;

        public DataWriterBase(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        protected byte[] ReadFromDisk(long pageStartOffset, int _pageSizeInBytes)
        {
            var retrievedPage = new byte[_pageSizeInBytes];
            _dataStorage.ReadData(ref retrievedPage, 0, pageStartOffset, _pageSizeInBytes);
            return retrievedPage;
        }

        protected long WriteToDisk(byte[] dataBuffer)
        {
            var endOfFile = _dataStorage.EndOfFileIndex;
            _dataStorage.WriteData(ref dataBuffer, 0, endOfFile, dataBuffer.Length);
            return endOfFile;
        }
    }

}
