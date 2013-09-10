using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kenny.DbEngine.Core.VirtualAddressSpace
{
    public class VirtualPage
    {
        private Guid id;
        private int size;
        private int used;
        private byte[] data;

        public VirtualPage(PageSummary summary)
        {
            id = Guid.NewGuid();
            size = summary.Size;
            used = 0;
            data = new byte[summary.Size];
        }

        public Guid Id
        {
            get { return id; }
        }

        public int Size
        {
            get { return size; }
        }

        public int Used
        {
            get { return used; }
        }
                
        public byte[] Data
        {
            get { return data; }
        }
    }
}
