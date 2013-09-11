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

        public DbEngineConfigurationSection()
        {

        }

        public static DbEngineConfigurationSection ConfigSection
        {
            get
            {
                if (config == null)
                {
                    config = (DbEngineConfigurationSection)ConfigurationManager.GetSection("DbEngine/DbEngineConfig");
                }
                return config;
            }
        }

        //private const string dbFileKey = "dbfilename";
        //[ConfigurationProperty("dbfilename",IsRequired=true)]
        //public string DbFileName
        //{
        //    get { return this[dbFileKey].ToString(); }
        //    set { this[dbFileKey] = value; }
        //}
    }
}
