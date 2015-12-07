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
        private SortedList<TKey, TData> _data = new SortedList<TKey, TData>();
        private SortedList<TKey, BPTreeNode<TKey, TData>> _children = new SortedList<TKey, BPTreeNode<TKey, TData>>();
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
                var selectedNode = _children.Values.Single(c => c.IsKeyInNodeRange(key));
                selectedNode.Insert(key, data);
            }
            return GetRoot();
        }

        public TData Query(TKey key)
        {
            throw new NotImplementedException();
            //if (_data.ContainsKey(key)) return _data[key];
            //else
            //{
            //    var childNode = _children.SingleOrDefault(c => c.IsKeyInNodeRange(key));
            //    return childNode == null ? default(TData) : childNode.Query(key);
            //}
        }

        private BPTreeNode<TKey, TData> GetRoot()
        {
            if (this.Parent == null) return this;
            else return Parent.GetRoot();
        }

        private void SplitNode()
        {
            var splitIndex = _children.Count / 2;
            splitIndex = _children.Count % 2 == 0 ? splitIndex : splitIndex++;
            var splitKey = _data.Keys[splitIndex];
            var splitData = _data.Values[splitIndex];

            
        }

        private void SendToParentNode(int index)
        {
            throw new NotImplementedException();
        }

        

        private bool IsKeyInNodeRange(TKey key)
        {
            throw new NotImplementedException();
            //if (_comparer.Compare(MaxKey, MinKey) == 0 && MinKey == null) return false;
            //if (MaxKey == null && MinKey != null && _comparer.Compare(MinKey, key) <= 0) return true;
            //if (MinKey == null && MaxKey != null && _comparer.Compare(MaxKey, key) > 0) return true;
            //if (MaxKey != null && MinKey != null && _comparer.Compare(MinKey, key) <= 0 && _comparer.Compare(MaxKey, key) >= 0) return true;
            //return false;
        }


        #endregion

        public delegate void NodeUpdatedDelgate(BPTreeNode<TKey, TData> nodeUpdated);
        public static event NodeUpdatedDelgate NodeUpdated;
    }

}
