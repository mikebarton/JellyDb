using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Spicy
{
    public class BPTreeNode<TKey,TData>
    {
        private int _branchingFactor = -1;
        private TKey _minKey;
        private TKey _maxKey;
        private SortedList<TKey, long> _data = new SortedList<TKey, long>();
        private SortedList<TKey, long> _children = new SortedList<TKey, long>();
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


        public TKey MaxKey { get; set; }


        public TKey MinKey { get; set; }

        public bool IsFull
        {
            get { return _data.Count >= BranchingFactor; }
        }

        public bool IsLeafNode
        {
            get { return !_children.Any(); }
        }
        #endregion


        #region Methods
        public BPTreeNode<TKey, TData> Insert(TKey key, TData data)
        {
            if (IsLeafNode)
            {
                _data[key] = data;
                if (IsFull) SplitNode();
            }
            else
            {
                var selectedNode = _children.Single(c => c.IsKeyInNodeRange(key));
                selectedNode.Insert(key, data);
            }
            return GetRoot();
        }

        public TData Query(TKey key)
        {
            if (_data.ContainsKey(key)) return _data[key];
            else
            {
                var childNode = _children.SingleOrDefault(c => c.IsKeyInNodeRange(key));
                return childNode == null ? default(TData) : childNode.Query(key);
            }
        }

        private BPTreeNode<TKey, TData> GetRoot()
        {
            if (this.Parent == null) return this;
            else return Parent.GetRoot();
        }

        private void SplitNode()
        {
            throw new NotImplementedException();
        }

        private void SendToParentNode(TKey key)
        {

        }


        #endregion

        public delegate void NodeUpdatedDelgate(BPTreeNode<TKey, TData> nodeUpdated);
        public static event NodeUpdatedDelgate NodeUpdated;
    }

}
