using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Spicy
{
    [Serializable]
    public class DataItem
    {        
        public long DataFileOffset { get; set; }
        public bool IsLeafNode { get; set; }
        public long DataLength { get; set; }
        
    }
}
