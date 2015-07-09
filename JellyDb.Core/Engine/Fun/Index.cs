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
    public class Index<TKey> : DataWritableBase, IIndex
    {
        private BPTreeNode<TKey, DataItem> _indexTree;

        public Index(IDataStorage dataStorage) : base(dataStorage)
        {
            Load();
            if(_indexTree == null)_indexTree = new BPTreeNode<TKey, DataItem>(15);
        }

        public void Insert(DataKey key, DataItem value)
        {
            _indexTree = _indexTree.Insert(key.GetKey<TKey>(), value);
        }

        public DataItem Query(DataKey key)
        {
            return _indexTree.Query(key.GetKey<TKey>());
        }
        
        public void SaveIndexToDisk()
        {
            var json = JsonConvert.SerializeObject(_indexTree);
            var bytes = ConvertDataToBytes(json);
            _dataStorage.ResetAddressSpace();
            WriteToDisk(bytes);
        }

        private void Load()
        {
            var dataBuffer = _dataStorage.ReadToEndOfAddressSpace(0);
            if(dataBuffer != null && dataBuffer.Length > 0)
            {
                var json = ConvertBytesToData(dataBuffer);
                var index = JsonConvert.DeserializeObject<BPTreeNode<TKey, DataItem>>(json);
                _indexTree = index;    
            }
        }

        public BPTreeNode<TKey,DataItem> IndexData
        {
            get { return _indexTree; }
            set { _indexTree = value; }
        }
    }
}
