using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Client
{
    public class JellyRecord<TEntity> : IJellyRecord
    {
        public string Id { get; set; }
        public TEntity Entity { get; set; }

        public Type GetEntityType()
        {
            return typeof(TEntity);
        }

        public string GetSerializedData()
        {
            throw new NotImplementedException();
        }

        public void GenerateKey(IKeyGenerator generator)
        {
            throw new NotImplementedException();
        }

    }
}
