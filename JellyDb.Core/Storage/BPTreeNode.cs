using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Storage
{
    public class BPTreeNode
    {
        private int _branchingFactor = 5;
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
                if (IsFull) return SplitNode();
            }
            else
            {
                var selectedNode = _children.Single(c => c.IsKeyInNodeRange(key));
                selectedNode.Insert(key, data);
            }
            return this;
        }

        private BPTreeNode SplitNode()
        {
            var splitIndex = (_branchingFactor - 1)/2;
            
        }

        public bool IsKeyInNodeRange(long key)
        {
            if (MaxKey == MinKey && MinKey == null) return false;
            if (MaxKey == null && MinKey != null && MinKey < key) return true;
            if (MinKey == null && MaxKey != null && MaxKey > key) return true;
            if (MaxKey != null && MinKey != null && MinKey < key && MaxKey > key) return true;
            return false;
        }
        
        public bool IsFull
        {
            get { return _data.Count >= _branchingFactor; }
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
