using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Spicy
{
    public class DatabaseNode<TKey>
    {
        private SortedList<TKey, long> _data = new SortedList<TKey, long>();
        private SortedList<TKey, long> _children = new SortedList<TKey, long>();
        


        public bool IsLeafNode
        {
            get { return !_children.Any(); }
        }
    }
}
