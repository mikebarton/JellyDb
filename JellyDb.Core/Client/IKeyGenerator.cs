using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Client
{
    public class IKeyGenerator
    {
        Type GetEntityType();
        object GenerateKey<TEntity>(TEntity entity);
    }
}
