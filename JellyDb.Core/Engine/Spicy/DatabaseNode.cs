using JellyDb.Core.VirtualAddressSpace.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JellyDb.Core.Engine.Spicy
{
    public class DatabaseNode<TKey>
    {
        private int _branchingFactor = -1;
        private TKey _minKey;
        private TKey _maxKey;        
        private SortedList<TKey, long> _data = new SortedList<TKey, long>();
        private SortedList<TKey, long> _children = new SortedList<TKey, long>();
        private IDataStorage _dataStorage;

        public DatabaseNode(int branchingFactor, IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
            _branchingFactor = branchingFactor;
        }

        public DatabaseNode<TKey> ReadNode(long address)
        {
            var result = new DatabaseNode<TKey>(_branchingFactor, _dataStorage);
            var branchFactorBytes = BitConverter.GetBytes(_branchingFactor);

            return result;
        }

        public void WriteNode()
        {

        }
    }
}
