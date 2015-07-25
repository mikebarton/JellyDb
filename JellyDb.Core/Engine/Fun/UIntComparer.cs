using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class UIntComparer : ITypeComparer<uint>
    {
        public uint MinKey 
        {
            get { return uint.MinValue; }            
        }

        public uint MaxKey
        {
            get { return uint.MaxValue; }            
        }

        public int Compare(uint one, uint two)
        {
            return one == two ? 0 
                : one < two ? -1 
                : 1;
        }

        public uint Decrement(uint input)
        {
            return input - 1; ;
        }

        public uint Increment(uint input)
        {
            return input + 1;
        }
    }
}
