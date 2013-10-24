using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JellyDb.Core.Storage
{
    public class InMemoryStorage : IDataStorage
    {
        private MemoryStream _data = new MemoryStream();
        public byte[] Read(long offset, long length)
        {
            byte[] data = new byte[length];
            _data.Read(data, (int)offset, (int)length);
            return data;
        }

        public void Write(long offset, long length, byte[] buffer, long startIndex)
        {
            var trimmedBuffer = buffer.Skip((int)startIndex).ToArray();
            _data.Write(trimmedBuffer, (int) offset, (int) length);
        }
    }
}
