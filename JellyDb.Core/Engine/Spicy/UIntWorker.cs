using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Storage;

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

        public uint Compare(uint first, uint second)
        {
            return first - second;
        }
    }
}
