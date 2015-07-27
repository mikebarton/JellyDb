using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Storage
{

    public abstract class StreamManager : IDataStorage
    {
        protected bool _flushRequired;
        protected abstract Stream Stream { get; }

        
        public abstract void Initialise();

        public virtual void Flush()
        {
            if (_flushRequired)
            {
                Stream.Flush();
                _flushRequired = false;
            }
        }

        public void WriteData(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToWrite)
        {
            Stream.Position = storageOffset;
            Stream.Write(dataBuffer, bufferIndex, numBytesToWrite);
            _flushRequired = true;
        }

        public void ReadData(ref byte[] dataBuffer, int bufferIndex, long storageOffset, int numBytesToRead)
        {
            Stream.Position = storageOffset;
            Stream.Read(dataBuffer, bufferIndex, numBytesToRead);
        }

        public byte[] ReadToEndOfAddressSpace(long storageOffset)
        {
            Stream.Position = storageOffset;
            var size = Stream.Length - storageOffset;
            if (size > Int32.MaxValue) throw new InvalidOperationException("Cannot perform ReadToEnd when length is great than Int32.MaxValue");
            var buffer = new byte[size];
            Stream.Read(buffer, 0, (int)size);
            return buffer;
        }

        public long EndOfFileIndex
        {
            get { return Stream.Length; }
        }

        public virtual void Dispose()
        {
            Flush();
            Stream.Close();
            Stream.Dispose();
        }


        public void ResetAddressSpace()
        {
            Stream.SetLength(0);
            Stream.Flush();
        }
    }
}
