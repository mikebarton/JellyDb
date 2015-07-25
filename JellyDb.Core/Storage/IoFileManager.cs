using JellyDb.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.ComponentModel;


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

        public override void Flush()
        {
            if (_flushRequired)
            {
                var fileStream = Stream as FileStream;
                fileStream.Flush();
#pragma warning disable 618,612 // disable stream.Handle deprecation warning.
                if (!FlushFileBuffers(fileStream.Handle))   // Flush OS file cache to disk.
#pragma warning restore 618,612
                {
                    Int32 err = Marshal.GetLastWin32Error();
                    throw new Win32Exception(err, "Win32 FlushFileBuffers returned error for " + fileStream.Name);
                }
                _flushRequired = false;
            }
        }


        [DllImport("kernel32", SetLastError = true)]
        private static extern bool FlushFileBuffers(IntPtr handle);    

    }
}
