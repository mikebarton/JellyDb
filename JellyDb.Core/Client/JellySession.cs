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
            var record = _jellyDatabase.OnLoadRecord<TKey,TEntity>(id);
            var entity = JsonConvert.DeserializeObject<TEntity>(record.GetSerializedData());
            return entity;
        }

        public T Query<T>(Expression<Func<T, bool>> whereClause)
        {
            throw new NotImplementedException();
        }

        public void Store<TEntity>(TEntity entity) where TEntity : class
        {
            var record = new JellyRecord<TEntity>(entity);
            _jellyDatabase.OnStoreRecord<TEntity>(record);
        }

        public void Dispose()
        {
            
        }
    }
}
