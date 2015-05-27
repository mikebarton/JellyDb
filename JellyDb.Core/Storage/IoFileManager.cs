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
        private static object _SyncObject = new object();
        
        protected override Stream Stream
        {
            get 
            {
                if (_stream == null)
                {
                    lock (_SyncObject)
                    {
                        if (_stream == null)
                        {
                            _stream = InitialiseStream();                            
                        }
                    }
                }
                return _stream;
            }
        }

        private Stream InitialiseStream()
        {
            var dbFileName = VirtualFileSystemConfigurationSection.ConfigSection.vfsFileName;
            return File.Open(dbFileName, FileMode.OpenOrCreate);
        }
    }
}
