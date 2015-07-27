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


        public bool ReadTypeFromDataSource(BinaryReaderWriter readerWriter, out ulong output)
        {
            output = readerWriter.ReadUInt64();
            return output != 0;
        }

        public void WriteEmptyObjectToDataSource(BinaryReaderWriter readerWriter)
        {
            readerWriter.Write(0);
        }
    }
}
