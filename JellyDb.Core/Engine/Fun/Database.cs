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
        private Dictionary<long, byte[]> _pageCache = new Dictionary<long, byte[]>();

        public Database(string databaseName)
        {
            _databaseName = databaseName;
            _indexRoot = Index.CreateOrLoad(databaseName);
        }

        public string Read(long key)
        {
            var page = _indexRoot.Query(key);
            var dataItem = page.Query(key);
            var totalData = RetrieveRemainderOfDataFromNextPage(new List<byte>(), page.DataFileOffset, dataItem.PageOffset, dataItem.ItemLength);

            var text = Encoding.Unicode.GetString(totalData.ToArray());
            return text;
        }

        private List<byte> RetrieveRemainderOfDataFromNextPage(List<byte> itemData, long dataFileOffset, int pageOffset, int itemLength)
        {
            byte[] pageData = null;
            if (!_pageCache.TryGetValue(dataFileOffset, out pageData))
            {
                var pageSizeInBytes = DataPage.PageSize * 1024;
                pageData = ReadFromDisk(dataFileOffset, pageSizeInBytes);
                _pageCache[dataFileOffset] = pageData;
            }
            var totalData = itemData.Concat(pageData.Skip(pageOffset).Take(itemLength)).ToList();
            if (totalData.Count < itemLength) totalData = RetrieveRemainderOfDataFromNextPage(totalData, dataFileOffset + DataPage.PageSize, 0, itemLength);
            return totalData;            
        }



        public void Write(long key, string data)
        {
            
        }

        public ReadDelegate ReadFromDisk { get; set; }
        public WriteDelegate WriteToDisk { get; set; }
    }

    public delegate byte[] ReadDelegate(long storageOffset, int numBytes);
    public delegate void WriteDelegate(long storageOffset, int bufferIndex, int numBytes, byte[] dataBuffer);
}
