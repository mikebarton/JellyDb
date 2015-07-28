using JellyDb.Core.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Spicy
{
    public class ULongWorker : ITypeWorker<ulong>
    {
        public byte[] GetBytes(ulong input)
        {
            return BitConverter.GetBytes(input);
        }

        public ulong GetTypedObject(byte[] input)
        {
            return BitConverter.ToUInt64(input, 0);
        }

        public int GetTypeSize()
        {
            return sizeof(ulong);
        }

        public ulong ReadTypeFromDataSource(BinaryReaderWriter readerWriter)
        {
            return readerWriter.ReadUInt64();
        }

        public void WriteTypeToDataSource(BinaryReaderWriter readerWriter, ulong input)
        {
            readerWriter.Write(input);
        }
        
        public void WriteEmptyObjectToDataSource(BinaryReaderWriter readerWriter)
        {
            readerWriter.Write(0);
        }

        public ulong Compare(ulong first, ulong second)
        {
            return first - second;
        }

    }
}
