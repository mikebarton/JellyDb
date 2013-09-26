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

        public string NodeName
        {
            get { return Key.ToString(); }
        }

        public void Insert(long key, long data)
        {
            if (IsLeafNode)
            {
                if (!IsFull)
                {
                    if (Children.ContainsKey(key)) Children[key].Data = data;
                    else
                        Children[key] = new BPTreeNode
                            {
                                Data = data,
                                Key = key,
                                Parent = this
                            };
                }
                else SplitNode();

                if (MinKey > key) MinKey = key;
                if (MaxKey < key) MaxKey = key;
            }
            else
            {
                var selectedNode = Children.SingleOrDefault(n => n.Value.IsKeyInNodeRange(key));
                if (!selectedNode.Equals(default(KeyValuePair<long, BPTreeNode>))) selectedNode.Value.Insert(key, data);
                else throw new InvalidOperationException("this node is note a leaf. need to insert into child, but there is no range with range that fits key");
            }
        }

        private void Insert(BPTreeNode node)
        {
            Children.Add(node.Key, node);
            if (IsFull) SplitNode();
        }

        private void SplitNode()
        {
            var splitIndex = (_branchingFactor - 1)/2;
            var firstChild = Children.ElementAt(0);
            var middleChild = Children.ElementAt(splitIndex);
            var leftBranch = new BPTreeNode {Key = firstChild.Key, Data = firstChild.Value.Data, Parent = this.Parent, MinKey = firstChild.Key, MaxKey = Children.ElementAt(splitIndex - 1).Key};
            var rightBranch = new BPTreeNode {Key = middleChild.Key, Data = middleChild.Value.Data, Parent = this.Parent, MinKey = middleChild.Key, MaxKey = Children.Last().Key};
            
            for (int i = 1; i < splitIndex; i++)
            {
                var movingNode = Children.ElementAt(i).Value;
                leftBranch.Insert(movingNode);
            }
            
            for (int i = splitIndex; i < Children.Count; i++)
            {
                var movingNode = Children.ElementAt(i).Value;
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
            get { return Children.Count >= _branchingFactor; }
        }

        public List<BPTreeNode> DisplayChildren
        {
            get { return Children.Values.ToList(); }
        }

        public SortedList<long, BPTreeNode> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        public bool IsLeafNode 
        {
            get { return !_children.Any(c => c.Value._children.Count > 0); }
        }
    }
}
