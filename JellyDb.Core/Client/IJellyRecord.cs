using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Client
{
    public interface IJellyRecord
    {
        Type GetEntityType();
        string GetSerializedData();
        TKey GenerateKey<TKey, TSource>(Func<TSource, TKey> generator);
    }
}
