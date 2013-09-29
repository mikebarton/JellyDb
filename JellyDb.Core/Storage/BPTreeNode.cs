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
            if (IsLeafNode) InsertNodeData(key, data);
            else
            {
                var selectedNode = _children.Single(c => c.IsKeyInNodeRange(key));
                selectedNode.Insert(key, data);
            }
            return GetRoot();
        }

        private BPTreeNode GetRoot()
        {
            if (this.Parent == null) return this;
            else return Parent.GetRoot();
        }

        private void InsertNodeData(long key, long data)
        {
            _data[key] = data;
            if (IsFull) SplitNode();
        }

        private long GetLargerSiblingMinKey(BPTreeNode target)
        {
            if (!_children.Contains(target)) throw new InvalidOperationException("Can only assign key range to a child node");

            
        }

        private void InsertChildNode(BPTreeNode node)
        {

        }

        private void SplitNode()
        {
            var splitIndex = (_branchingFactor - 1)/2;
            var left = this;
            var right = new BPTreeNode() { Parent = this.Parent };

            var splitElem = _data.ElementAt(splitIndex);
            left.MaxKey = splitElem.Key - 1;
            right.MinKey = splitElem.Key;
            right.MaxKey = Parent.GetLargerSiblingMinKey(right) - 1;

            for (int i = _data.Count-1; i >= splitIndex; i--)
            {
                var elem = _data.ElementAt(i);
                right.Insert(elem.Key, elem.Value);
                _data.RemoveAt(i);
            }            

            var childSplitElem = _children.OrderBy(c => c.MinKey).Last(c => c.MinKey < splitElem.Key);
            var childSplitIndex = _children.IndexOf(childSplitElem);

            for (int i = _children.Count; i >= childSplitIndex; i--)
            {
                right.Children.Add(_children[i]);
                _children.RemoveAt(i);
            }
            Parent.InsertNodeData(splitElem.Key, splitElem.Value);
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
