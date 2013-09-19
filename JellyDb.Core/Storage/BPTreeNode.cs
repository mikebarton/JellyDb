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
        public long? MaxKey { get; set; }
        public long? MinKey { get; set; }


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

        private void Insert(BPTreeNode node)
        {
            _children.Add(node.Key, node);
            if (IsFull) SplitNode();
        }

        private void SplitNode()
        {
            var splitIndex = (_branchingFactor - 1)/2;
            var firstChild = _children.ElementAt(0);
            var middleChild = _children.ElementAt(splitIndex);
            var leftBranch = new BPTreeNode {Key = firstChild.Key, Data = firstChild.Value.Data, Parent = this.Parent, MinKey = firstChild.Key, MaxKey = _children.ElementAt(splitIndex - 1).Key};
            var rightBranch = new BPTreeNode {Key = middleChild.Key, Data = middleChild.Value.Data, Parent = this.Parent, MinKey = middleChild.Key, MaxKey = _children.Last().Key};
            
            for (int i = 1; i < splitIndex; i++)
            {
                var movingNode = _children.ElementAt(i).Value;
                leftBranch.Insert(movingNode);
            }
            
            for (int i = splitIndex; i < _children.Count; i++)
            {
                var movingNode = _children.ElementAt(i).Value;
                rightBranch.Insert(movingNode);
            }
            
            if (Parent == null) Parent = new BPTreeNode() {Key = middleChild.Key, Data = middleChild.Value.Data};
            Parent.Insert(leftBranch);
            Parent.Insert(rightBranch);

            Parent = leftBranch.IsKeyInNodeRange(Key) ? leftBranch : rightBranch;
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
            get { return _children.Count >= _branchingFactor; }
        }
    }
}
