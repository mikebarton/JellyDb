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

        public bool ReadTypeFromDataSource(BinaryReaderWriter readerWriter, out uint output)
        {
            output = readerWriter.ReadUInt32();
            return output != 0;
        }

        public void WriteEmptyObjectToDataSource(BinaryReaderWriter readerWriter)
        {
            readerWriter.Write(0);
        }
    }
}
