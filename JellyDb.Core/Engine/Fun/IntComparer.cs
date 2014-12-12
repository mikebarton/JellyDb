using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class IntComparer : ITypeComparer<int>
    {
        public int MinKey 
        {
            get { return int.MinValue; }            
        }

        public int MaxKey
        {
            get { return int.MaxValue; }            
        }

        public int Compare(int one, int two)
        {
            return one == two ? 0 : one < two ? -1 : 1;
        }

        public int Decrement(int input)
        {
            return input - 1; ;
        }

        public int Increment(int input)
        {
            return input + 1;
        }
    }
}
