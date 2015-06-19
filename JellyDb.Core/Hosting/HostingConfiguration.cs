using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Hosting
{
    public class HostingConfiguration
    {
        public string ServerFolderRoot { get; set; }
        public HostingType HostingType { get; set; }
        public string ConnectionString { get; set; }
        public int PageSizeInKb { get; set; }
        public int PageIncreaseNum { get; set; }
    }

    public enum HostingType
    {
        FileBased,
        ServerBased
    }
}
