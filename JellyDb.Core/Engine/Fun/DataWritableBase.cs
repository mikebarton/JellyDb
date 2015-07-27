using JellyDb.Core.Configuration;
using JellyDb.Core.VirtualAddressSpace;
using JellyDb.Core.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class DataWritableBase : DataWriterBase
    {
        private static byte[] _startBytes = new byte[] { 0xAF };
        private static byte[] _endBytes = new byte[] { 0xFA, 0xAF };

        public DataWritableBase(IDataStorage dataStorage) : base(dataStorage)
        {}

        protected static string ConvertBytesToData(byte[] dataBuffer)
        {
            if (!dataBuffer.Take(_startBytes.Length).SequenceEqual(_startBytes)) throw new InvalidDataException("Data File is Corrupt. When reading data item. data boundary start markers did not align.");
            if (!dataBuffer.Skip(dataBuffer.Length - _endBytes.Length).Take(_endBytes.Length).SequenceEqual(_endBytes)) throw new InvalidDataException("Data File is Corrupt. When reading data item, data boundary end markers did not align.");

            var strippedData = dataBuffer.Skip(_startBytes.Length).Take(dataBuffer.Length - _startBytes.Length - _endBytes.Length);
            var data = Encoding.Unicode.GetString(strippedData.ToArray());
            return data;
        }

        protected static byte[] ConvertDataToBytes(string data)
        {
            var dataBytes = Encoding.Unicode.GetBytes(data);
            var totalData = _startBytes.Concat(dataBytes).Concat(_endBytes).ToArray();
            return totalData;
        }            
    }    
}
