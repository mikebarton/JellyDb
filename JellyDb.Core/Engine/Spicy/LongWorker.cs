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

        public bool ReadTypeFromDataSource(BinaryReaderWriter readerWriter, out long output)
        {
            output = readerWriter.ReadInt64();
            return output != 0;
        }

        public void WriteEmptyObjectToDataSource(BinaryReaderWriter readerWriter)
        {
            readerWriter.Write(0);
        }
    }
}
