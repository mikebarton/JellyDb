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
        private long _storageOffset;
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
            result._storageOffset = address;
            _readerWriter.SetPosition(address);
            result._branchingFactor = _readerWriter.ReadInt32();
            result._minKey = _typeWorker.ReadTypeFromDataSource(_readerWriter);
            result._maxKey = _typeWorker.ReadTypeFromDataSource(_readerWriter);
            for (int i = 0; i < _branchingFactor; i++)
            {
                var key = _typeWorker.ReadTypeFromDataSource(_readerWriter);
                var dataAddress = _readerWriter.ReadInt64();
                result._data.Add(key, dataAddress);
            }

            for (int i = 0; i < _branchingFactor; i++)
            {
                var key = _typeWorker.ReadTypeFromDataSource(_readerWriter);
                var childAddress = _readerWriter.ReadInt64();
                result._children.Add(key, childAddress);
            }
            
            return result;
        }

        public void WriteNode()
        {
            _readerWriter.SetPosition(_storageOffset);
            _readerWriter.Write(_branchingFactor);
            _typeWorker.WriteTypeToDataSource(_readerWriter, _minKey);
            _typeWorker.WriteTypeToDataSource(_readerWriter, _maxKey);
            for (int i = 0; i < _branchingFactor; i++)
            {
                if (i < _data.Count)
                {
                    _typeWorker.WriteTypeToDataSource(_readerWriter, _data.Keys[i]);
                    _readerWriter.Write(_data.Values[i]);
                }
                else
                {
                    _typeWorker.WriteTypeToDataSource(_readerWriter, 0);
                }
            }
        }
    }
}
