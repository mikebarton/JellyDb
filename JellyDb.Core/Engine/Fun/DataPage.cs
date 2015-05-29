using JellyDb.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class DataPage : BPTreeNode<long, DataItem>
    {
        public long DataFileOffset { get; set; }
        public static int PageSize { get; set; }
        public Guid Id { get; set; }

        static DataPage()
        {
            PageSize = DbEngineConfigurationSection.ConfigSection.VfsConfig.PageSizeInKb;
        }
    }
}
