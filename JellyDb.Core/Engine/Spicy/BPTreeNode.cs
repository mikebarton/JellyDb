using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Extensions;

namespace JellyDb.Core.Engine.Spicy
{
    public class BPTreeNode<TKey,TData>
    {
        private int _branchingFactor = -1;
        private TKey _minKey;
        private TKey _maxKey;
        //private SortedList<TKey, TData> _data = new SortedList<TKey, TData>();
        //private SortedList<TKey, BPTreeNode<TKey, TData>> _children = new SortedList<TKey, BPTreeNode<TKey, TData>>();
        private SortedList<TKey, NodeItem<TKey, TData>> _items = new SortedList<TKey, NodeItem<TKey, TData>>();
        private ITypeWorker<TKey> _typeWorker;

        public BPTreeNode(int branchingFactor = 15)
        {
            _branchingFactor = branchingFactor;
            _typeWorker = TypeWorkerFactory.GetTypeWorker<TKey>();
        }

        #region Properties
        public int BranchingFactor
        {
            get { return _branchingFactor; }
            set
            {
                //if (_branchingFactor != -1) throw new InvalidOperationException("once branching factor is defined for a node it cannot be redefined");
                _branchingFactor = value;
            }
        }

        public BPTreeNode<TKey, TData> Parent { get; set; }


        //public TKey MaxKey { get; set; }


        //public TKey MinKey { get; set; }
        internal SortedList<TKey, NodeItem<TKey, TData>> Items { get { return _items; } }
       
        public bool IsFull
        {
            get { return _items.Count >= BranchingFactor; }
        }

        public bool IsLeafNode
        {
            get { return !_items.Any(i=>i.Value.Child != null); }
        }
        #endregion


        #region Methods
        public BPTreeNode<TKey, TData> Insert(TKey key, TData data)
        {
            if (IsLeafNode)
            {
                _items[key] = new NodeItem<TKey, TData> { Key = key, Data = data };                
                if (IsFull) SplitNode();
            }
            else
            {
                var selectedNode = FindRelevantNodeItem(key);
                //var selectedNode = _children.Values.Single(c => c.IsKeyInNodeRange(key));
                selectedNode.Child.Insert(key, data);
            }
            return GetRoot();
        }

        public TData Query(TKey key)
        {
            if (_items.ContainsKey(key)) return _items[key].Data;
            else
            {
                var childNode = FindRelevantNodeItem(key);
                //var childNode = _children.SingleOrDefault(c => c.IsKeyInNodeRange(key));
                return childNode.Child == null ? default(TData) : childNode.Child.Query(key);
            }
        }
        //TODO: when storing key less than current node key need to create child node with minkey int.minvalue///
        private NodeItem<TKey, TData> FindRelevantNodeItem(TKey key)
        {
            var selectedNode = _typeWorker.Compare(key, _items.First().Key) < 0 ?
                    _items.First() :
                    _items.LastOrDefault(i => _typeWorker.Compare(key, i.Value.Key) > 0);
            return selectedNode.Value;
        }

        private BPTreeNode<TKey, TData> GetRoot()
        {
            if (this.Parent == null) return this;
            else return Parent.GetRoot();
        }

        private void SplitNode()
        {
            var splitIndex = _items.Count / 2;
            var splitNode = _items.Values[splitIndex];

            _items.Remove(splitNode.Key);
            if (Parent == null) Parent = new BPTreeNode<TKey, TData>(BranchingFactor);
            var parentMinKey = Parent.GetMinKey();
            if (parentMinKey != null && _typeWorker.Compare(splitNode.Key, parentMinKey) < 0)
            {
                splitNode.IsMinimumKey = true;
                //TODO change this. what do i do when splitting here
            }

            var largerSiblings = _items.Values.Where(i => _typeWorker.Compare(i.Key, splitNode.Key) > 0).ToArray();
            var newChildNode = new BPTreeNode<TKey, TData>(BranchingFactor);

            if (this.IsLeafNode)
            {
                newChildNode._items.Add(splitNode.Key, splitNode);
                largerSiblings.ForEach(x =>
                {
                    newChildNode._items.Add(x.Key, x);
                    _items.Remove(x.Key);
                });
            }
            else
            {
                largerSiblings.ForEach(x =>
                {
                    newChildNode._items.Add(x.Key, x);
                    _items.Remove(x.Key);
                });
                if (splitNode.Child != null)
                {
                    var splitChild = splitNode.Child;
                    splitNode.Child = null;
                    var parentMaxKey = Parent.GetMaxKey();
                    if(parentMaxKey == null) parentMaxKey = splitNode.Key;
                    newChildNode._items.Add(parentMaxKey, new NodeItem<TKey, TData> { Key = parentMaxKey, Child = splitChild });
                }
            }
            splitNode.Child = newChildNode;

            Parent.SendToParentNode(splitNode);            
        }

        private void SendToParentNode(NodeItem<TKey, TData> nodeItem)
        {
            if (_items.ContainsKey(nodeItem.Key)) throw new InvalidOperationException("Attempt to split node. When sending node to parent, the key already existed in the parent");
            _items[nodeItem.Key] = nodeItem;
            if (IsFull) SplitNode();
        }

        private TKey GetMinKey()
        {
            return _items.Keys.FirstOrDefault();
        }

        private TKey GetMaxKey()
        {
            return _items.Keys.LastOrDefault();
        }
        
        #endregion

        public delegate void NodeUpdatedDelgate(BPTreeNode<TKey, TData> nodeUpdated);
        public static event NodeUpdatedDelgate NodeUpdated;

        internal class NodeItem<TKey, TData>
        {
            internal TKey Key { get; set; }
            internal bool IsMinimumKey { get; set; }
            internal TData Data { get; set; }
            internal BPTreeNode<TKey, TData> Child { get; set; }
        }
    }


    
}
