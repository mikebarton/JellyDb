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
    public class Index : DataWritableBase
    {
        private BPTreeNode<long, DataItem> _indexTree;

        public Index(IDataStorage dataStorage) : base(dataStorage)
        {
            _indexTree = new BPTreeNode<long, DataItem>(15);
        }

        public void Insert(long key, DataItem value)
        {
            _indexTree = _indexTree.Insert(key, value);
        }

        public DataItem Query(long key)
        {
            return _indexTree.Query(key);
        }
        
        public void SaveIndexToDisk()
        {
            var json = JsonConvert.SerializeObject(this);
            var bytes = ConvertDataToBytes(json);
            WriteToDisk(bytes);
        }

        public static Index Load(byte[] dataBuffer)
        {
            var json = ConvertBytesToData(dataBuffer);
            var index = JsonConvert.DeserializeObject<Index>(json);
            return index;             
        }

        public BPTreeNode<long,DataItem> IndexData
        {
            get { return _indexTree; }
            set { _indexTree = value; }
        }
    }
}
