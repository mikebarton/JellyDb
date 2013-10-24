using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    [Serializable]
    public class IndexResult
    {
        public long Offset { get; set; }
        public long Size { get; set; }
    }
}
