using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class LongComparer : ITypeComparer<ulong>
    {
        public ulong MinKey 
        {
            get { return ulong.MinValue; }            
        }

        public ulong MaxKey
        {
            get { return ulong.MaxValue; }            
        }

        public int Compare(ulong one, ulong two)
        {
            return one == two ? 0 
                : one < two ? -1 
                : 1;
        }

        public ulong Decrement(ulong input)
        {
            return input - 1; ;
        }

        public ulong Increment(ulong input)
        {
            return input + 1;
        }
    }
}
