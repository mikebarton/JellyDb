using JellyDb.Core.VirtualAddressSpace.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JellyDb.Core.Extensions;
using JellyDb.Core.Configuration;

namespace JellyDb.Core.Engine.Fun
{
    public class Database : DataWritableBase, IDisposable
    {
        private IIndex _indexRoot;
        private Dictionary<long, byte[]> _pageCache = new Dictionary<long, byte[]>();
        private static int _pageSizeInBytes = DbEngineConfigurationSection.ConfigSection.VfsConfig.PageSizeInKb * 1024;

        public Database(IIndex index, IDataStorage dataStorage)
            : base(dataStorage)
        {
            _indexRoot = index;
        }

        public string Read(DataKey key)
        {
            var dataItem = _indexRoot.Query(key);
            var totalData = RetrieveItemData(new List<byte>(), dataItem.DataFileOffset, dataItem.PageOffset, dataItem.ItemLength).ToArray();
            var text = ConvertBytesToData(totalData);
            return text;
        }

        private List<byte> RetrieveItemData(List<byte> itemData, long dataFileOffset, int pageOffset, int itemLength, bool isContinuance = false)
        {
            var pageStartOffset = dataFileOffset - pageOffset;
            byte[] pageData = null;
            if (!_pageCache.TryGetValue(pageStartOffset, out pageData))
            {
                pageData = ReadFromDisk(pageStartOffset, _pageSizeInBytes);
                _pageCache[pageStartOffset] = pageData;
            }
            var totalData = itemData.Concat(pageData.Skip(isContinuance ? 0 : pageOffset).Take(itemLength)).ToList();
            if (totalData.Count < itemLength) totalData = RetrieveItemData(totalData, dataFileOffset + _pageSizeInBytes, pageOffset, itemLength - (totalData.Count - itemData.Count), true);
            return totalData;            
        }

        public void Write(DataKey key, string data)
        {
            var dataItem = new DataItem() { VersionId = Guid.NewGuid() };
            var dataBuffer = ConvertDataToBytes(data);
            dataItem.ItemLength = dataBuffer.Length;
            var dataFileOffset = WriteToDisk(dataBuffer);
            dataItem.PageOffset = (dataFileOffset % _pageSizeInBytes).TruncateToInt32();
            dataItem.DataFileOffset = dataFileOffset;
            _indexRoot.Insert(key, dataItem);
        }        
        
        public void Dispose()
        {
            this.Flush();
            _indexRoot.SaveIndexToDisk();
        }
    }    
}
