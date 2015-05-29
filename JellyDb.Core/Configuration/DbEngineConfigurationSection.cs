using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace JellyDb.Core.Configuration
{
    public class DbEngineConfigurationSection : ConfigurationSection
    {
        private static DbEngineConfigurationSection config;
        
        public static DbEngineConfigurationSection ConfigSection
        {
            get
            {
                if (config == null)
                {
                    config = (DbEngineConfigurationSection)ConfigurationManager.GetSection("JellyDb");
                }
                return config;
            }
        }

        private const string folderPathKey = "folderpath";
        [ConfigurationProperty("folderpath", IsRequired = true)]
        public string FolderPath
        {
            get { return this[folderPathKey].ToString(); }
        }

        private const string vfsConfigKey = "VirtualFileSystemConfig";
        [ConfigurationProperty("VirtualFileSystemConfig", IsRequired = true)]
        public VirtualFileSystemConfigurationSection VfsConfig
        {
            get { return this[vfsConfigKey] as VirtualFileSystemConfigurationSection; }
        }
    }
}
