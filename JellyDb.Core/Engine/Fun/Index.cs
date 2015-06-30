using JellyDb.Core.Configuration;
using JellyDb.Core.VirtualAddressSpace.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class Index<TKey> : DataWritableBase
    {
        private BPTreeNode<TKey, DataItem> _indexTree;

        public Index(IDataStorage dataStorage) : base(dataStorage)
        {
            _indexTree = new BPTreeNode<TKey, DataItem>(15);
        }

        public void Insert(TKey key, DataItem value)
        {
            _indexTree = _indexTree.Insert(key, value);
        }

        public DataItem Query(TKey key)
        {
            return _indexTree.Query(key);
        }
        
        public void SaveIndexToDisk()
        {
            var json = JsonConvert.SerializeObject(this);
            var bytes = ConvertDataToBytes(json);
            WriteToDisk(bytes);
        }

        public static Index<TKey> Load(byte[] dataBuffer)
        {
            var json = ConvertBytesToData(dataBuffer);
            var index = JsonConvert.DeserializeObject<Index<TKey>>(json);
            return index;             
        }

        public BPTreeNode<TKey,DataItem> IndexData
        {
            get { return _indexTree; }
            set { _indexTree = value; }
        }
    }
}
