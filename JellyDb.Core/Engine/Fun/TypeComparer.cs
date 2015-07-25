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
            _specificComparer = GetTypeComparer();
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

        public static ITypeComparer<TKey> GetTypeComparer()
        {
            if (typeof(TKey) == typeof(int)) return (ITypeComparer<TKey>)new IntComparer();
            else if (typeof(TKey) == typeof(uint)) return (ITypeComparer<TKey>)new UIntComparer();
            else if (typeof(TKey) == typeof(ulong) || typeof(TKey) == typeof(ulong)) return (ITypeComparer<TKey>)new LongComparer();
            else throw new NotSupportedException("type not supported: " + typeof(TKey).FullName);
        }
    }
}
