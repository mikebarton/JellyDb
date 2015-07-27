using JellyDb.Core.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Storage
{
    public class MemoryStreamManager : StreamManager
    {
        private MemoryStream _stream;

        protected override System.IO.Stream Stream
        {
            get { return _stream; }
        }

        public override void Initialise()
        {
            _stream = new MemoryStream();
        }
    }
}
