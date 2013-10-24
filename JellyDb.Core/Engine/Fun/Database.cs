using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Storage;

namespace JellyDb.Core.Engine.Fun
{
    public class Database
    {
        private BPTreeNode<long, IndexResult> _indexRoot;
        private IDataStorage _dataStorage;

        public Database()
        {
            
        }

        private void InitializeNewDataStorage()
        {
            _dataStorage = new InMemoryStorage();
            var indexSize = _dataStorage.Read(0, 4);//read first int
            //BitConverter.ToInt32(indexSize);
        }

        private void WriteIndex()
        {
            
        }
    }
}
