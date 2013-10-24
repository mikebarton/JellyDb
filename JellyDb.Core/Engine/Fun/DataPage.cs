using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class DataPage
    {
        public static long PageSize { get; set; }
        public long Offset { get; set; }
        private BPTreeNode<long, IndexResult> _pageIndex;

        static DataPage()
        {
            PageSize = 1024*8;
        }
    }
}
