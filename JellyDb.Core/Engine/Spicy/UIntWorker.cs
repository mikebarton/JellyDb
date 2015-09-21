using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Storage;
using JellyDb.Core.Extensions;

namespace JellyDb.Core.Engine.Spicy
{
    public class UIntWorker : ITypeWorker<uint>
    {
        public byte[] GetBytes(uint input)
        {
            return BitConverter.GetBytes(input);
        }

        public uint GetTypedObject(byte[] input)
        {
            return BitConverter.ToUInt32(input, 0);
        }

        public int GetTypeSize()
        {
            return sizeof(uint);
        }

        
        public void WriteTypeToDataSource(BinaryReaderWriter readerWriter, uint input)
        {
            readerWriter.Write(input);
        }

        public uint ReadTypeFromDataSource(BinaryReaderWriter readerWriter)
        {
            var output = readerWriter.ReadUInt32();
            return output;
        }

        public void WriteEmptyObjectToDataSource(BinaryReaderWriter readerWriter)
        {
            readerWriter.Write(0);
        }

        public int Compare(uint first, uint second)
        {
            if (first < second) return -1;
            if (first > second) return 1;
            return 0;
        }


        public uint Decrement(uint input)
        {
            return input - 1;
        }

        public uint Increment(uint input)
        {
            return input + 1;
        }
    }
}
