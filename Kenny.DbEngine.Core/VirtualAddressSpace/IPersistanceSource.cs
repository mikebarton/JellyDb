using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kenny.DbEngine.Core.Configuration;

namespace Kenny.DbEngine.Core.VirtualAddressSpace
{
    public  interface IPersistanceSource
    {
        byte[] GetData(Guid addressSpaceId, long offset, long numBytes);
        void SetData(Guid addressSpaceId, long offset, long startIndex, long numBytes, byte[] dataArray);
    }
}
