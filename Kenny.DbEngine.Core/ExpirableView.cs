using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;

namespace Kenny.DbEngine.Core
{
    internal class ExpirableView : ExpirableBase
    {
        private MemoryMappedViewAccessor view;
        private long offset;
        private long size;
     
        internal ExpirableView(int secondsUntilExpiration, long offset, MemoryMappedViewAccessor view, long size)
            : base(secondsUntilExpiration)
        {
            this.view = view;
            this.offset = offset;
            this.size = size;
        }

        internal override void Expire()
        {
            view.Flush();
            view.Dispose();
        }

        public long Offset
        {
            get { return offset; }
        }

        public MemoryMappedViewAccessor View
        {
            get { return view; }
        }


    }
}
