using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class LongComparer : ITypeComparer<long>
    {
        public long MinKey 
        {
            get { return int.MinValue; }            
        }

        public long MaxKey
        {
            get { return int.MaxValue; }            
        }

        public int Compare(long one, long two)
        {
            return one == two ? 0 
                : one < two ? -1 
                : 1;
        }

        public long Decrement(long input)
        {
            return input - 1; ;
        }

        public long Increment(long input)
        {
            return input + 1;
        }
    }
}
