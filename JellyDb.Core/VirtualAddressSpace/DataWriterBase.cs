using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace
{
    public class DataWriterBase
    {   
        internal ReadDelegate ReadFromDisk { get; set; }        
        internal WriteDelegate WriteToDisk { get; set; }
    }
    public delegate byte[] ReadDelegate(long storageOffset, int numBytes);
    public delegate long WriteDelegate(byte[] dataBuffer);

}
