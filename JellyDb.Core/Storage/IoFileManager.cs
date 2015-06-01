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
        private Stream _stream;

        protected override Stream Stream { get { return _stream; } }

        public override void Initialise()
        {
            var folderName = DbEngineConfigurationSection.ConfigSection.FolderPath;
            var dbFileName = Path.Combine(folderName, "dbFile.dat");
            _stream = File.Open(dbFileName, FileMode.OpenOrCreate);
        }
    }
}
