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

        public uint ReadUInt32()
        {
            var buffer = new byte[4];
            _dataStorage.ReadData(ref buffer, 0, _storageOffset, 4);
            _storageOffset += 4;
            return BitConverter.ToUInt32(buffer, 0);
        }

        public long ReadInt64()
        {
            var buffer = new byte[8];
            _dataStorage.ReadData(ref buffer, 0, _storageOffset, 8);
            _storageOffset += 8;
            return BitConverter.ToInt64(buffer, 0);
        }

        public ulong ReadUInt64()
        {
            var buffer = new byte[8];
            _dataStorage.ReadData(ref buffer, 0, _storageOffset, 8);
            _storageOffset += 8;
            return BitConverter.ToUInt64(buffer, 0);
        }

        public void Write(int number)
        {
            var bytes = BitConverter.GetBytes(number);
            if (bytes.Length != 4) throw new InvalidProgramException("byte array from Int32 is not 4 bytes long. I have misunderstood something");
            _dataStorage.WriteData(ref bytes, 0, _storageOffset, 4);
            _storageOffset += 4;
        }

        public void Write(uint number)
        {
            var bytes = BitConverter.GetBytes(number);
            if (bytes.Length != 4) throw new InvalidProgramException("byte array from UInt32 is not 4 bytes long. I have misunderstood something");
            _dataStorage.WriteData(ref bytes, 0, _storageOffset, 4);
            _storageOffset += 4;
        }

        public void Write(long number)
        {
            var bytes = BitConverter.GetBytes(number);
            if (bytes.Length != 8) throw new InvalidProgramException("byte array from Int64 is not 8 bytes long. I have misunderstood something");
            _dataStorage.WriteData(ref bytes, 0, _storageOffset, 8);
            _storageOffset += 8;
        }

        public void Write(ulong number)
        {
            var bytes = BitConverter.GetBytes(number);
            if (bytes.Length != 8) throw new InvalidProgramException("byte array from UInt64 is not 8 bytes long. I have misunderstood something");
            _dataStorage.WriteData(ref bytes, 0, _storageOffset, 8);
            _storageOffset += 8;
        }
    }
}
