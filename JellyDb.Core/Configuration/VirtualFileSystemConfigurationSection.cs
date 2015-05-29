using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace JellyDb.Core.Configuration
{
    public class VirtualFileSystemConfigurationSection : ConfigurationElement
    {
        private const string pageSizeKbKey = "pagesizekb";
        [ConfigurationProperty("pagesizekb", DefaultValue = 1024)]
        public int PageSizeInKb
        {
            get { return (int)this[pageSizeKbKey]; }
            set { this[pageSizeKbKey] = value; }
        }

        private const string pageIncreaseKey = "pagesincreasenum";
        [ConfigurationProperty("pagesincreasenum", DefaultValue = 5)]
        public int PageIncreaseNum
        {
            get { return (int)this[pageIncreaseKey]; }
            set { this[pageIncreaseKey] = value; }
        }
    }
}
