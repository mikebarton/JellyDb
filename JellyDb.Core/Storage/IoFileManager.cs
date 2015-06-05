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
        private string _filePath;

        public IoFileManager(string filePath)
        {
            _filePath = filePath;
        }

        protected override Stream Stream { get { return _stream; } }

        public override void Initialise()
        {
            _stream = File.Open(_filePath, FileMode.OpenOrCreate);
        }
    }
}
