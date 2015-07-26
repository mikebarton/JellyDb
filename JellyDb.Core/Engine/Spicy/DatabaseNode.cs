using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace JellyDb.Core.Engine.Spicy
{
    public class DatabaseNode<TKey>
    {
        private int _branchingFactor = -1;
        private TKey _minKey;
        private TKey _maxKey;        
        private SortedList<TKey, long> _data = new SortedList<TKey, long>();
        private SortedList<TKey, long> _children = new SortedList<TKey, long>();
        

        public DatabaseNode(int branchingFactor)
        {
            
            _branchingFactor = branchingFactor;
        }

        public bool IsLeafNode
        {
            get { return !_children.Any(); }
        }
    }
}
