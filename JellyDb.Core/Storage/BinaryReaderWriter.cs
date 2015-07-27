using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Storage
{
    public class BinaryReaderWriter
    {
        private IDataStorage _dataStorage;
        private long _storageOffset = 0;
        public BinaryReaderWriter(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public void SetPosition(long position)
        {
            _storageOffset = position;
        }        

        public int ReadInt32()
        {
            var buffer = new byte[4];
            _dataStorage.ReadData(ref buffer, 0, _storageOffset, 4);
            _storageOffset += 4;
            return BitConverter.ToInt32(buffer, 0);
        }

        public void Write(int number)
        {
            var bytes = BitConverter.GetBytes(number);
            if (bytes.Length != 4) throw new InvalidProgramException("byte array from Int32 is not 4 bytes long. I have misunderstood something");
            _dataStorage.WriteData(ref bytes, 0, _storageOffset, 4);
            _storageOffset += 4;
        }
    }
}
