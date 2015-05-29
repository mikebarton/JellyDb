using JellyDb.Core.VirtualAddressSpace.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class Database
    {
        private BPTreeNode<long, DataPage> _indexRoot;
        private IDataStorage _dataStorage;

        public Database(IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public string Read(long key)
        {
            
        }

        public void Write(long key, string data)
        {
            
        }
    }
}
