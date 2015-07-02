using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace JellyDb.Core.Client
{
    public class JellySession : IDisposable
    {
        private JellyDatabase _jellyDatabase;

        internal JellySession(JellyDatabase jellyDatabase)
        {
            _jellyDatabase = jellyDatabase;
        }

        public TEntity Load<TKey, TEntity>(TKey id)
        {
            var record = _jellyDatabase.OnLoadRecord<TKey>(id);
            var entity = JsonConvert.DeserializeObject<TEntity>(record.Data);
            return entity;
        }

        public T Query<T>(Expression<Func<T, bool>> whereClause)
        {
            throw new NotImplementedException();
        }

        public string Store<TKey>(TKey entity) 
        {
            var entityTypeName = entity.GetType().FullName;
            var text = JsonConvert.SerializeObject(entity);
            var record = new JellyRecord { EntityType = entityTypeName, Data = text };
            _jellyDatabase.OnStoreRecord<TKey>(record);
            return record.Id;
        }

        public void Dispose()
        {
            
        }
    }
}
