using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Client
{
    public class JellyRecord<TEntity>
    {
        public string Id { get; set; }
        public TEntity Entity { get; set; }

        

        public string GetSerializedData()
        {
            throw new NotImplementedException();
        }
    }
}
