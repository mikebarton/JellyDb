﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace JellyDb.Core.Engine.Fun
{
    [Serializable][XmlRoot(ElementName = "N")]
    public class BPTreeNode<TKey, TData>
    {
        private int _branchingFactor = -1;
        public BPTreeNode(int branchingFactor) : this()
        {
            this._branchingFactor = branchingFactor;
        }

        [XmlIgnore]
        public int BranchingFactor
        {
            get { return _branchingFactor; }
            set 
            {
                if (_branchingFactor != -1) throw new InvalidOperationException("once branching factor is defined for a node it cannot be redefined");
                _branchingFactor = value; 
            }
        }
        private List<BPTreeNode<TKey, TData>> _children = new List<BPTreeNode<TKey, TData>>();
        private SortedList<TKey, TData> _data = new SortedList<TKey, TData>();
        private ITypeComparer<TKey> _comparer;

        public BPTreeNode()
        {
            _comparer = new TypeComparer<TKey>();
        }
        
        [XmlIgnore]
        public BPTreeNode<TKey, TData> Parent { get; set; }

        [XmlElement(ElementName = "M")]
        public TKey MaxKey { get; set; }

        [XmlElement(ElementName = "m")]
        public TKey MinKey { get; set; }


        public BPTreeNode<TKey, TData> Insert(TKey key, TData data)
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

        private void InsertChildNode(BPTreeNode<TKey, TData> node, TKey key, TData data)
        {
            _data[key] = data;
            node.Parent = this;
            var keyIndex = _data.IndexOfKey(key);

            var grandChildKey = node.Data.Keys.First();
            if (_comparer.Compare( grandChildKey, key) < 0)
            {
                node.MaxKey = _comparer.Decrement(key);
                if (keyIndex > 0) node.MinKey = _data.Keys.ElementAt(--keyIndex);
                else node.MinKey = _comparer.MinKey;
            }
            else
            {
                node.MinKey = key;
                if (keyIndex < _data.Count - 1) node.MaxKey = _data.Keys.ElementAt(++keyIndex);
                else node.MaxKey = _comparer.MaxKey;    
            }

            _children.Add(node);
            if (IsFull) SplitNode();
        }

        private void SplitNode()
        {
            var splitIndex = (BranchingFactor - 1) / 2;
            var left = this;
            var right = new BPTreeNode<TKey, TData>(BranchingFactor);

            var splitElem = _data.ElementAt(splitIndex);
            left.MaxKey = _comparer.Decrement(splitElem.Key);

            for (int i = _data.Count-1; i >= splitIndex; i--)
            {
                var elem = _data.ElementAt(i);
                right.Insert(elem.Key, elem.Value);
                _data.RemoveAt(i);
            }   
            
            foreach (var child in _children.Where(c=>_comparer.Compare( c.MinKey, splitElem.Key) >= 0).ToList())
            {
                right.Children.Add(child);
                child.Parent = right;
                _children.Remove(child);
            }

            if (this.Parent == null)
            {
                this.Parent = new BPTreeNode<TKey, TData>(BranchingFactor);
                this.Parent.InsertChildNode(left, splitElem.Key, splitElem.Value);
            }
            Parent.InsertChildNode(right, splitElem.Key, splitElem.Value);
        }

        public bool IsKeyInNodeRange(TKey key)
        {
            if (_comparer.Compare(MaxKey, MinKey) == 0 && MinKey == null) return false;
            if (MaxKey == null && MinKey != null && _comparer.Compare(MinKey, key) <= 0) return true;
            if (MinKey == null && MaxKey != null && _comparer.Compare(MaxKey,key) > 0) return true;
            if (MaxKey != null && MinKey != null && _comparer.Compare(MinKey, key) <= 0 && _comparer.Compare(MaxKey, key) > 0) return true;
            return false;
        }
        
        public bool IsFull
        {
            get { return _data.Count >= BranchingFactor; }
        }

        [XmlArray(ElementName = "C")]
        [XmlArrayItem(ElementName = "N")]
        public List<BPTreeNode<TKey, TData>> Children
        {
            get { return _children; }
            set { _children = value; }
        }

        [XmlIgnore]
        public SortedList<TKey, TData> Data
        {
            get { return _data; }
        }

        [XmlArray(ElementName = "S")]
        [XmlArrayItem(ElementName = "d")]
        public List<TKey> SerializableData
        {
            get { return Data.Keys.ToList(); }
        }

        public bool IsLeafNode 
        {
            get { return !_children.Any(); }
        }
    }
}
