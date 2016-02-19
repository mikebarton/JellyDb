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

        public int ReadTypeFromDataSource(BinaryReaderWriter readerWriter)
        {
            var output = readerWriter.ReadInt32();
            return output;
        }        

        public void WriteTypeToDataSource(BinaryReaderWriter readerWriter, int input)
        {
            readerWriter.Write(input);
        }

        public void WriteEmptyObjectToDataSource(BinaryReaderWriter readerWriter)
        {
            readerWriter.Write(0);
        }


        public int Compare(int first, int second)
        {
            if (first < second) return -1;
            if (first > second) return 1;
            return 0;
        }

        public int Decrement(int input)
        {
            return input - 1;
        }

        public int Increment(int input)
        {
            return input + 1;
        }


        public int MinKey
        {
            get { return -1; }
        }
    }
}
