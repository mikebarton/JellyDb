using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Kenny.DbEngine.Core.Configuration
{
    public class VirtualFileSystemConfigurationSection : ConfigurationSection
    {
        private static VirtualFileSystemConfigurationSection config;

        public VirtualFileSystemConfigurationSection()
        {

        }

        public static VirtualFileSystemConfigurationSection ConfigSection
        {
            get
            {
                if (config == null)
                {
                    config = (VirtualFileSystemConfigurationSection)ConfigurationManager.GetSection("DbEngine/VirtualFileSystemConfig");
                }
                return config;
            }
        }

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
        
        private const string dbFileKey = "vfsfilename";
        [ConfigurationProperty("vfsfilename", IsRequired = true)]
        public string vfsFileName
        {
            get { return this[dbFileKey].ToString(); }
            set { this[dbFileKey] = value; }
        }
    }
}
