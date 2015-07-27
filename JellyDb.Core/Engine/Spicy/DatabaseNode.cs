using JellyDb.Core.Storage;
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
        private ITypeWorker<TKey> _typeWorker;
        private BinaryReaderWriter _readerWriter;

        public DatabaseNode(int branchingFactor, IDataStorage dataStorage)
        {
            _dataStorage = dataStorage;
            _readerWriter = new BinaryReaderWriter(_dataStorage);
            _branchingFactor = branchingFactor;
            _typeWorker = TypeWorkerFactory.GetTypeWorker<TKey>();
        }

        public DatabaseNode<TKey> ReadNode(long address)
        {
            var result = new DatabaseNode<TKey>(_branchingFactor, _dataStorage);
            _readerWriter.SetPosition(address);
            result._branchingFactor = _readerWriter.ReadInt32();
            result._minKey = _typeWorker.ReadTypeFromDataSource(_readerWriter);
            result._maxKey = _typeWorker.ReadTypeFromDataSource(_readerWriter);
            
            return result;
        }

        public void WriteNode()
        {
            var branchFactorBytes = BitConverter.GetBytes(_branchingFactor);
            var minKeyBytes = _typeWorker.GetBytes(_minKey);
            var maxKeyBytes = _typeWorker.GetBytes(_maxKey);
        }
    }
}
