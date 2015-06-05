using JellyDb.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public abstract class DataWritableBase
    {
        private static byte[] _startBytes = new byte[] { 0xAF };
        private static byte[] _endBytes = new byte[] { 0xFA };

        protected virtual string ConvertBytesToData(int numBytes, byte[] dataBuffer)
        {
            if (!dataBuffer.Take(_startBytes.Length).SequenceEqual(_startBytes)) throw new InvalidDataException(string.Format("Data File is Corrupt. When reading data item {0}, data boundary start markers did not align.", dataItem.PageOffset));
            if (!dataBuffer.Skip(numBytes - _startBytes.Length).Take(_endBytes.Length).SequenceEqual(_endBytes)) throw new InvalidDataException(string.Format("Data File is Corrupt. When reading data item {0}, data boundary end markers did not align.", dataItem.PageOffset));

            var strippedData = dataBuffer.Skip(_startBytes.Length).Take(numBytes - _startBytes.Length - _endBytes.Length);
            var data = Encoding.Unicode.GetString(strippedData.ToArray());
            return data;
        }

        protected virtual byte[] ConvertDataToBytes(out int numBytes, string data)
        {
            var dataBytes = Encoding.Unicode.GetBytes(data);
            var totalData = _startBytes.Concat(dataBytes).Concat(_endBytes).ToArray();
            numBytes = totalData.Length;
            return totalData;
        }        

        public ReadDelegate ReadFromDisk { get; set; }
        public WriteDelegate WriteToDisk { get; set; }
    }
    public delegate byte[] ReadDelegate(long storageOffset, int numBytes);
    public delegate long WriteDelegate(byte[] dataBuffer);
}
