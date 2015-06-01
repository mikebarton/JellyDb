using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    [Serializable]
    public class DataItem
    {
        public long DataFileOffset { get; set; }
        public int PageOffset { get; set; }
        public int ItemLength { get; set; }
        public Guid VersionId { get; set; }
    }
}
