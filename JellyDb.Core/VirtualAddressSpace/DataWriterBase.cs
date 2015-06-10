using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.VirtualAddressSpace
{
    public class DataWriterBase
    {        
        [JsonIgnore]
        public ReadDelegate ReadFromDisk { get; set; }
        [JsonIgnore]
        public WriteDelegate WriteToDisk { get; set; }
    }
    public delegate byte[] ReadDelegate(long storageOffset, int numBytes);
    public delegate long WriteDelegate(byte[] dataBuffer);
}
