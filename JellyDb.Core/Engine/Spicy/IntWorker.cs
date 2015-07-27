using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Storage;

namespace JellyDb.Core.Engine.Spicy
{
    public class IntWorker : ITypeWorker<int>
    {
        public byte[] GetBytes(int input)
        {
            return BitConverter.GetBytes(input);
        }

        public int GetTypedObject(byte[] input)
        {
            return BitConverter.ToInt32(input, 0);
        }

        public int GetTypeSize()
        {
            return sizeof(int);
        }

        public bool ReadTypeFromDataSource(BinaryReaderWriter readerWriter, out int output)
        {
            output = readerWriter.ReadInt32();
            return output != 0;
        }        

        public void WriteTypeToDataSource(BinaryReaderWriter readerWriter, int input)
        {
            readerWriter.Write(input);
        }

        public void WriteEmptyObjectToDataSource(BinaryReaderWriter readerWriter)
        {
            readerWriter.Write(0);
        }
    }
}
