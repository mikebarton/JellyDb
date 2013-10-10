using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public interface ITypeComparer<TKey>
    {
        TKey MinKey { get; }
        TKey MaxKey { get; }
        int Compare(TKey one, TKey two);
        TKey Decrement(TKey input);
        TKey Increment(TKey input);
    }
}
