using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class StringComparer : ITypeComparer<string>
    {
        public string MinKey
        {
            get { throw new NotImplementedException(); }
        }

        public string MaxKey
        {
            get { throw new NotImplementedException(); }
        }

        public int Compare(string one, string two)
        {
            throw new NotImplementedException();
        }

        public string Decrement(string input)
        {
            throw new NotImplementedException();
        }

        public string Increment(string input)
        {
            throw new NotImplementedException();
        }
    }
}
