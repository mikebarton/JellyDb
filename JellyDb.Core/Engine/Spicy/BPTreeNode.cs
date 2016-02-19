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
        private SortedList<TKey, NodeItem<TKey, TData>> _items = new SortedList<TKey, NodeItem<TKey, TData>>();
        private ITypeWorker<TKey> _typeWorker;

        public BPTreeNode(int branchingFactor = 15)
        {
            _branchingFactor = branchingFactor;
            _typeWorker = TypeWorkerFactory.GetTypeWorker<TKey>();
        }
        
        public int BranchingFactor
        {
            get { return _branchingFactor; }
            set
            {
                _branchingFactor = value;
            }
        }

        public BPTreeNode<TKey, TData> Parent { get; set; }


        internal SortedList<TKey, NodeItem<TKey, TData>> Items { get { return _items; } }
       
        public bool IsFull
        {
            get { return _items.Count >= BranchingFactor; }
        }

        public bool IsLeafNode
        {
            get { return !_items.Any(i=>i.Value.Child != null); }
        }
        
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
                return childNode.Child == null ? default(TData) : childNode.Child.Query(key);
            }
        }
        //TODO: when storing key less than current node key need to create child node with minkey int.minvalue///
        private NodeItem<TKey, TData> FindRelevantNodeItem(TKey key)
        {
            var first = _items.FirstOrDefault();
            if (first.Value == null) return null;

            var selectedNode = _typeWorker.Compare(key, _items.First().Key) < 0 ?
                    _items.FirstOrDefault() :
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
            splitIndex = _items.Count % 2 == 0 ? splitIndex-1 : splitIndex;
            var splitItem = _items.Values[splitIndex];
            _items.Remove(splitItem.Key);

            if (Parent == null) Parent = new BPTreeNode<TKey, TData>(BranchingFactor);

            var largerSiblings = _items.Values.Skip(splitIndex).ToList();
            splitItem.Child = new BPTreeNode<TKey, TData>(BranchingFactor);
           
            largerSiblings.ForEach(s =>
            {
                splitItem.Child.Insert(s);
                _items.Remove(s.Key);
            });
            Parent.Insert(splitItem);

            if (Parent.GetRelatedNodeItem(this) == null)
            {
                var newItem = new NodeItem<TKey, TData> { Key = _typeWorker.MinKey, Child = this };
                Parent.Insert(newItem);
            }
        }

        private NodeItem<TKey,TData> GetRelatedNodeItem(BPTreeNode<TKey, TData> node)
        {
            return _items.Values.SingleOrDefault(x => x.Child == node);
        }

        private void Insert(NodeItem<TKey, TData> nodeItem)
        {
            if (_items.ContainsKey(nodeItem.Key)) throw new InvalidOperationException("Attempt to split node. When sending node to parent, the key already existed in the parent");
            _items[nodeItem.Key] = nodeItem;
            if (nodeItem.Child != null) nodeItem.Child.Parent = this;
            if (IsFull) SplitNode();
        }

        #region VisualizationData

        public List<BPTreeNode<TKey, TData>> Children
        {
            get { return _items.Values.Select(x => x.Child).Where(x => x != null).ToList(); }
        }

        public TKey MinKey
        {
            get { return _items.Keys.FirstOrDefault(); }
        }

        public TKey MaxKey
        {
            get { return _items.Keys.LastOrDefault(); }
        }

        public List<TData> Data
        {
            get { return _items.Values.Select(x => x.Data).ToList(); }
        }

        public List<TKey> Key
        {
            get { return _items.Keys.ToList(); }
        }


        #endregion
        
        internal class NodeItem<TKey, TData>
        {
            internal TKey Key { get; set; }
            internal TData Data { get; set; }
            internal BPTreeNode<TKey, TData> Child { get; set; }
        }
    }


    
}
