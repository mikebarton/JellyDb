using JellyDb.Core.Extensions;
using JellyDb.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Spicy
{
    public class LongWorker : ITypeWorker<long>
    {
        public byte[] GetBytes(long input)
        {
            return BitConverter.GetBytes(input);
        }

        public long GetTypedObject(byte[] input)
        {
            return BitConverter.ToInt64(input, 0);
        }

        public int GetTypeSize()
        {
            return sizeof(long);
        }

        
        public void WriteTypeToDataSource(BinaryReaderWriter readerWriter, long input)
        {
            readerWriter.Write(input);
        }

        public long ReadTypeFromDataSource(BinaryReaderWriter readerWriter)
        {
            var output = readerWriter.ReadInt64();
            return output;
        }

        public void WriteEmptyObjectToDataSource(BinaryReaderWriter readerWriter)
        {
            readerWriter.Write(0);
        }

        public int Compare(long first, long second)
        {
            if (first < second) return -1;
            if (first > second) return 1;
            return 0;
        }

        
    }
}
