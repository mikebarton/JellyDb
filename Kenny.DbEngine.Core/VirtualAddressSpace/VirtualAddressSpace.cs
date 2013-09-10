using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kenny.DbEngine.Core.IoC;

namespace Kenny.DbEngine.Core.VirtualAddressSpace
{
    public abstract class VirtualAddressSpace
    {
        private IPersistData persistence;

        public VirtualAddressSpace()
        {
            persistence = new FileDataPersistence();
        }

        public long Length()
        {
            throw new NotImplementedException();
        }

        public byte[] ReadBytes(int startPositionm, int length)
        {
            throw new NotImplementedException();
        }

    }
}
