using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;

namespace Kenny.DbEngine.Core.VirtualAddressSpace
{
    public class ViewContainer
    {
        public MemoryMappedViewAccessor View { get; set; }
        public bool IsDirty { get; set; }
        public long StorageOffset { get; set; }
        public long StorageLength { get; set; }
    }
}
