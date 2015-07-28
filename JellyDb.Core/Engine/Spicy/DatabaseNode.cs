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

        public void Insert(TKey key, DataItem data)
        {
            if (_children.Any())
            {
                var indexOfChild = _data.IndexOfKey( _data.Keys.Last(d => _typeWorker.Compare(key, d) < 0));
                var selectedNodeAddress = _children.Values[indexOfChild];
                var selectedNode = ReadNodeFromDataSource(selectedNodeAddress);
                selectedNode.Insert(key, data);                
            }
            else
            {
                //TODO write DataItem to storage
                //_data[key] = data;
                //if (IsFull) SplitNode();
            }
        }

        public DatabaseNode<TKey> ReadNodeFromDataSource(long address)
        {
            var result = new DatabaseNode<TKey>(_branchingFactor, _dataStorage);
            result._storageOffset = address;
            _readerWriter.SetPosition(address);
            result._branchingFactor = _readerWriter.ReadInt32();
            result._minKey = _typeWorker.ReadTypeFromDataSource(_readerWriter);
            result._maxKey = _typeWorker.ReadTypeFromDataSource(_readerWriter);
            var numData = _readerWriter.ReadInt32();
            for (int i = 0; i < _branchingFactor; i++)
            {
                if (i < numData)
                {
                    var key = _typeWorker.ReadTypeFromDataSource(_readerWriter);
                    var dataAddress = _readerWriter.ReadInt64();
                    result._data.Add(key, dataAddress);
                }
                else
                {
                    _typeWorker.ReadTypeFromDataSource(_readerWriter);
                    _readerWriter.ReadInt64();
                }
            }

            var numChildren = _readerWriter.ReadInt32();
            for (int i = 0; i < _branchingFactor; i++)
            {
                if (i < numChildren)
                {
                    var key = _typeWorker.ReadTypeFromDataSource(_readerWriter);
                    var childAddress = _readerWriter.ReadInt64();
                    result._children.Add(key, childAddress);
                }
                else
                {
                    _typeWorker.ReadTypeFromDataSource(_readerWriter);
                    _readerWriter.ReadInt64();
                }
            }
            
            return result;
        }

        public void WriteNodeToDataSource()
        {
            _readerWriter.SetPosition(_storageOffset);
            _readerWriter.Write(_branchingFactor);
            _typeWorker.WriteTypeToDataSource(_readerWriter, _minKey);
            _typeWorker.WriteTypeToDataSource(_readerWriter, _maxKey);
            _readerWriter.Write(_data.Count);
            for (int i = 0; i < _branchingFactor; i++)
            {
                if (i < _data.Count)
                {
                    _typeWorker.WriteTypeToDataSource(_readerWriter, _data.Keys[i]);
                    _readerWriter.Write(_data.Values[i]);
                }
                else
                {
                    _typeWorker.WriteEmptyObjectToDataSource(_readerWriter);
                    _readerWriter.Write((long)0);
                }
            }

            _readerWriter.Write(_children.Count);
            for (int i = 0; i < _branchingFactor; i++)
            {
                if (i < _children.Count)
                {
                    _typeWorker.WriteTypeToDataSource(_readerWriter, _children.Keys[i]);
                    _readerWriter.Write(_children.Values[i]);
                }
                else
                {
                    _typeWorker.WriteEmptyObjectToDataSource(_readerWriter);
                    _readerWriter.Write((long)0);
                }
            }
        }
    }
}
