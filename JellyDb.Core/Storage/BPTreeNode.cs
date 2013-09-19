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
            if (!IsLeafNode)
            {
                var prevNode = _children.First(n => 

            }
            if (IsLeafNode)
            {
                if (_children.ContainsKey(key)) _children[key].Data = data;
                else _children[key] = new BPTreeNode() { Parent = this, Key = key, Data = data };

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
