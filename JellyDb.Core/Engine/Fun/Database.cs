using JellyDb.Core.VirtualAddressSpace.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JellyDb.Core.Extensions;

namespace JellyDb.Core.Engine.Fun
{
    public class Database
    {
        private Index _indexRoot;
        private string _databaseName;
        private Dictionary<long, byte[]> _pageCache = new Dictionary<long, byte[]>();
        private const byte[] _startBytes = new byte[] { 0xAF };
        private const  byte[] _endBytes = new byte[] { 0xFA };


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
            var dataItem = new DataItem() { VersionId = Guid.NewGuid() };
            var dataBuffer = ConvertDataToBytes(dataItem, data);
            var dataFileOffset = WriteToDisk(dataBuffer);
            dataItem.PageOffset = (dataFileOffset % (DataPage.PageSize * 1024)).TruncateToInt32();
            var currentPage = _indexRoot.Query(dataFileOffset);
            if (currentPage == null) currentPage = new DataPage();
            currentPage.DataFileOffset = dataFileOffset;
            currentPage.Insert(dataFileOffset, dataItem);
            _indexRoot.Insert(dataFileOffset, currentPage);
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
            if (!dataBuffer.Skip(dataItem.ItemLength - _startBytes.Length - _endBytes.Length).Take(_endBytes.Length).SequenceEqual(_endBytes)) throw new InvalidDataException(string.Format("Data File is Corrupt. When reading data item {0}, data boundary end markers did not align.", dataItem.PageOffset));

            var strippedData = dataBuffer.Skip(_startBytes.Length).Take(dataItem.ItemLength = _startBytes.Length - _endBytes.Length);
            var data = Encoding.Unicode.GetString(dataBuffer);
            return data;
        }

        public ReadDelegate ReadFromDisk { get; set; }
        public WriteDelegate WriteToDisk { get; set; }
    }

    public delegate byte[] ReadDelegate(long storageOffset, int numBytes);
    public delegate long WriteDelegate(byte[] dataBuffer);
}
