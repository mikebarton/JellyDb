using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Storage
{
    public class BPTreeNode
    {
        private int _branchingFactor = 5;
        private SortedList<long, BPTreeNode> _children = new SortedList<long, BPTreeNode>();

        public BPTreeNode()
        {
            MinKey = long.MinValue;
            MaxKey = long.MaxValue;
        }
        
        public BPTreeNode Parent { get; set; }
        public long Key { get; set; }
        public long Data { get; set; }
        public long MaxKey { get; set; }
        public long MinKey { get; set; }


        public void Insert(long key, long data)
        {
            var selectedNode = _children.SingleOrDefault(n => n.Value.IsKeyInNodeRange(key));
            if (!selectedNode.Equals(default(KeyValuePair<long, BPTreeNode>))) selectedNode.Value.Insert(key, data);
            else
            {
                if (_children.ContainsKey(key)) _children[key].Data = data;
                else _children[key] = new BPTreeNode
                {
                    Data = data,
                    Key = key,
                    Parent = this
                };
                if (MinKey > key) MinKey = key;
                if (MaxKey < key) MaxKey = key;
                if (IsFull) SplitNode();
            }
        }

        private void SplitNode()
        {
            throw new NotImplementedException();
        }

        public void Insert(BPTreeNode node)
        {
            
        }

        public bool IsKeyInNodeRange(long key)
        {
            if (MaxKey == MinKey == null) return false;
            if (MaxKey == null && MinKey != null && MinKey < key) return true;
            if (MinKey == null && MaxKey != null && MaxKey > key) return true;
            if (MaxKey != null && MinKey != null && MinKey < key && MaxKey > key) return true;
            return false;
        }

        public bool IsLeafNode
        {
            get { return _children.All(n => n.Value._children.Count == 0); }
        } 

        public bool IsFull
        {
            get { return _children.Count >= _branchingFactor; }
        }
    }
}
