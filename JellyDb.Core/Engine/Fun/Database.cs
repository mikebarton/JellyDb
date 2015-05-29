using JellyDb.Core.VirtualAddressSpace.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class Database
    {
        private Index _indexRoot;
        private string _databaseName;
        private Dictionary<Guid, byte[]> _pageCache = new Dictionary<Guid, byte[]>();

        public Database(string databaseName)
        {
            _databaseName = databaseName;
            _indexRoot = Index.CreateOrLoad(databaseName);
        }

        public string Read(long key)
        {
            var page = _indexRoot.Query(key);
            byte[] pageData = null;
            if (!_pageCache.TryGetValue(page.Id, out pageData))
            {
                var pageSizeInBytes = DataPage.PageSize * 1024;
                pageData = ReadFromDisk(page.DataFileOffset, pageSizeInBytes);
                _pageCache[page.Id] = pageData;
            }
            var dataItem = page.Query(key);
            //TODO: retrieve data from page
            //TODO need to handle case of data overlapping multiple pages
        }

        public void Write(long key, string data)
        {
            
        }

        public ReadDelegate ReadFromDisk { get; set; }
        public WriteDelegate WriteToDisk { get; set; }
    }

    internal delegate byte[] ReadDelegate(long storageOffset, int numBytes);
    internal delegate void WriteDelegate(long storageOffset, int bufferIndex, int numBytes, byte[] dataBuffer);
}
