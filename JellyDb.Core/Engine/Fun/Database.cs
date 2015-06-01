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
    public class Database : IDisposable
    {
        private Index _indexRoot;
        private string _databaseName;
        private Dictionary<long, byte[]> _pageCache = new Dictionary<long, byte[]>();
        private static byte[] _startBytes = new byte[] { 0xAF };
        private static byte[] _endBytes = new byte[] { 0xFA };
        private static int _pageSizeInBytes = DbEngineConfigurationSection.ConfigSection.VfsConfig.PageSizeInKb * 1024;

        public Database(string databaseName)
        {
            _databaseName = databaseName;
            _indexRoot = Index.CreateOrLoad(databaseName);
        }

        public string Read(long key)
        {
            var dataItem = _indexRoot.Query(key);
            var totalData = RetrieveItemData(new List<byte>(), dataItem.DataFileOffset, dataItem.PageOffset, dataItem.ItemLength).ToArray();

            var text = ConvertBytesToData(dataItem, totalData);
            return text;
        }

        private List<byte> RetrieveItemData(List<byte> itemData, long dataFileOffset, int pageOffset, int itemLength)
        {
            var pageStartOffset = dataFileOffset - pageOffset;
            byte[] pageData = null;
            if (!_pageCache.TryGetValue(pageStartOffset, out pageData))
            {
                pageData = ReadFromDisk(pageStartOffset, _pageSizeInBytes);
                _pageCache[pageStartOffset] = pageData;
            }
            var totalData = itemData.Concat(pageData.Skip(pageOffset).Take(itemLength)).ToList();
            if (totalData.Count < itemLength) totalData = RetrieveItemData(totalData, dataFileOffset + _pageSizeInBytes, 0, itemLength);
            return totalData;            
        }



        public void Write(long key, string data)
        {
            var dataItem = new DataItem() { VersionId = Guid.NewGuid() };
            var dataBuffer = ConvertDataToBytes(dataItem, data);
            var dataFileOffset = WriteToDisk(dataBuffer);
            dataItem.PageOffset = (dataFileOffset % _pageSizeInBytes).TruncateToInt32();
            dataItem.DataFileOffset = dataFileOffset;
            _indexRoot.Insert(key, dataItem);
        }

        private byte[] ConvertDataToBytes(DataItem dataItem, string data)
        {            
            var dataBytes = Encoding.Unicode.GetBytes(data);
            var totalData = _startBytes.Concat(dataBytes).Concat(_endBytes).ToArray();
            dataItem.ItemLength = totalData.Length;
            return totalData;
        }

        private string ConvertBytesToData(DataItem dataItem, byte[] dataBuffer)
        {
            if (!dataBuffer.Take(_startBytes.Length).SequenceEqual(_startBytes)) throw new InvalidDataException(string.Format("Data File is Corrupt. When reading data item {0}, data boundary start markers did not align.", dataItem.PageOffset));
            if (!dataBuffer.Skip(dataItem.ItemLength - _startBytes.Length).Take(_endBytes.Length).SequenceEqual(_endBytes)) throw new InvalidDataException(string.Format("Data File is Corrupt. When reading data item {0}, data boundary end markers did not align.", dataItem.PageOffset));

            var strippedData = dataBuffer.Skip(_startBytes.Length).Take(dataItem.ItemLength - _startBytes.Length - _endBytes.Length);
            var data = Encoding.Unicode.GetString(strippedData.ToArray());
            return data;
        }

        public ReadDelegate ReadFromDisk { get; set; }
        public WriteDelegate WriteToDisk { get; set; }

        public void Dispose()
        {
            _indexRoot.SaveIndexToDisk();
        }
    }

    public delegate byte[] ReadDelegate(long storageOffset, int numBytes);
    public delegate long WriteDelegate(byte[] dataBuffer);
}
