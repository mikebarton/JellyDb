using JellyDb.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace.Storage
{
    public class IoFileManager : StreamManager
    {
        private static object _SyncObject = new object();
        private Stream _stream;

        protected override Stream Stream { get { return _stream; } }

        public override void Initialise(string databaseName)
        {
            var folderName = DbEngineConfigurationSection.ConfigSection.FolderPath;
            var dbFileName = Path.Combine(folderName, string.Format("{0}.dat", databaseName));
            _stream = File.Open(dbFileName, FileMode.OpenOrCreate);
        }
    }
}
