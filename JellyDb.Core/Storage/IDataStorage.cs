using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Storage
{
    public interface IDataStorage
    {
        byte[] Read(long offset, long length);
        void Write(long offset, long length, byte[] buffer, long startIndex);
    }
}
