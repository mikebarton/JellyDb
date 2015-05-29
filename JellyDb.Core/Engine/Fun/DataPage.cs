using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class DataPage : BPTreeNode<long, string>
    {
        public long DataFileOffset { get; set; }

    }
}
