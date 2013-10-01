using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Storage
{
    public class BPTreeNode
    {
        private int _branchingFactor = -1;
        public BPTreeNode(int branchingFactor)
        {
            this._branchingFactor = branchingFactor;
        }

        public int BranchingFactor
        {
            get { return _branchingFactor; }
            set 
            {
                if (_branchingFactor != -1) throw new InvalidOperationException("once branching factor is defined for a node it cannot be redefined");
                _branchingFactor = value; 
            }
        }
        private List<BPTreeNode> _children = new List<BPTreeNode>();
        private SortedList<long, long> _data = new SortedList<long, long>();

        public BPTreeNode()
        {
            MinKey = long.MinValue;
            MaxKey = long.MaxValue;
        }
        
        public BPTreeNode Parent { get; set; }
        public long? MaxKey { get; set; }
        public long? MinKey { get; set; }


        public BPTreeNode Insert(long key, long data)
        {
            if (IsLeafNode)
            {
                _data[key] = data;
                if(IsFull) SplitNode();
            }
            else
            {
                var selectedNode = _children.Single(c => c.IsKeyInNodeRange(key));
                selectedNode.Insert(key, data);
            }
            return GetRoot();
        }

        public long? Query(long key)
        {
            if (_data.ContainsKey(key)) return _data[key];
            else
            {
                var childNode = _children.SingleOrDefault(c => c.IsKeyInNodeRange(key));
                return childNode == null ? null : childNode.Query(key);
            }
        }

        private BPTreeNode GetRoot()
        {
            if (this.Parent == null) return this;
            else return Parent.GetRoot();
        }
        
        private void InsertChildNode(BPTreeNode node, long key, long data)
        {
            _data[key] = data;
            node.Parent = this;
            var keyIndex = _data.IndexOfKey(key);

            var grandChildKey = node.Data.Keys.First();
            if (grandChildKey < key)
            {
                node.MaxKey = key - 1;
                if (keyIndex > 0) node.MinKey = _data.Keys.ElementAt(--keyIndex);
                else node.MinKey = long.MinValue;
            }
            else
            {
                node.MinKey = key;
                if (keyIndex < _data.Count - 1) node.MaxKey = _data.Keys.ElementAt(++keyIndex);
                else node.MaxKey = long.MaxValue;    
            }

            _children.Add(node);
            if (IsFull) SplitNode();
        }

        private void SplitNode()
        {
            var splitIndex = (BranchingFactor - 1) / 2;
            var left = this;
            var right = new BPTreeNode(BranchingFactor);

            var splitElem = _data.ElementAt(splitIndex);
            left.MaxKey = splitElem.Key - 1;

            for (int i = _data.Count-1; i >= splitIndex; i--)
            {
                var elem = _data.ElementAt(i);
                right.Insert(elem.Key, elem.Value);
                _data.RemoveAt(i);
            }   
            
            foreach (var child in _children.Where(c=>c.MinKey >= splitElem.Key).ToList())
            {
                right.Children.Add(child);
                child.Parent = right;
                _children.Remove(child);
            }

            if (this.Parent == null)
            {
                this.Parent = new BPTreeNode(BranchingFactor);
                this.Parent.InsertChildNode(left, splitElem.Key, splitElem.Value);
            }
            Parent.InsertChildNode(right, splitElem.Key, splitElem.Value);
        }

        public bool IsKeyInNodeRange(long key)
        {
            if (MaxKey == MinKey && MinKey == null) return false;
            if (MaxKey == null && MinKey != null && MinKey <= key) return true;
            if (MinKey == null && MaxKey != null && MaxKey >= key) return true;
            if (MaxKey != null && MinKey != null && MinKey <= key && MaxKey >= key) return true;
            return false;
        }
        
        public bool IsFull
        {
            get { return _data.Count >= BranchingFactor; }
        }
        
        public List<BPTreeNode> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public SortedList<long,long> Data
        {
            get { return _data; }
        }

        public bool IsLeafNode 
        {
            get { return !_children.Any(); }
        }
    }
}
