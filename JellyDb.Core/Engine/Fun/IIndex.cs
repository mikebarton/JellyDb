using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public interface IIndex
    {
        void Insert(DataKey key, DataItem value);
        DataItem Query(DataKey key);
        void SaveIndexToDisk();
    }
}
