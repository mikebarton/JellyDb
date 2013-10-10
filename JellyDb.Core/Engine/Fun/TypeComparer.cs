using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class TypeComparer<TKey> : ITypeComparer<TKey>
    {
        private ITypeComparer<TKey> _specificComparer;
        public TypeComparer()
        {
            if (typeof(TKey) == typeof(int)) _specificComparer = (ITypeComparer<TKey>)new IntComparer();
            else throw new NotSupportedException("type not supported: " + typeof(TKey).FullName);
        }

        public TKey MinKey
        {
            get { return _specificComparer.MinKey; }
        }

        public TKey MaxKey
        {
            get { return _specificComparer.MaxKey; }
        }

        public int Compare(TKey one, TKey two)
        {
            return _specificComparer.Compare(one, two);
        }

        public TKey Decrement(TKey input)
        {
            return _specificComparer.Decrement(input);
        }

        public TKey Increment(TKey input)
        {
            return _specificComparer.Increment(input);
        }
    }
}
