using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class DataKey
    {
        private object _key;
        
        public TKey GetKey<TKey>()
        {
            return (TKey) _key;
        }

        public static DataKey CreateKey<TKey>(TKey key)
        {
            return new DataKey(){_key = key};
        }
    }
}
