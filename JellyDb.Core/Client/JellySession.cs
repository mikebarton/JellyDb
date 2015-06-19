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
        public T Load<T>(string id)
        {
            var record = LoadRecord(id);
            var entity = JsonConvert.DeserializeObject<T>(record.Data);
            return entity;
        }

        public T Query<T>(Expression<Func<T, bool>> whereClause)
        {
            throw new NotImplementedException();
        }

        public string Store<T>(T entity) where T : class
        {
            var entityTypeName = entity.GetType().FullName;
            var text = JsonConvert.SerializeObject(entity);
            var record = new JellyRecord { EntityType = entityTypeName, Data = text };
            StoreRecord(record);
            return record.Id;
        }

        internal delegate JellyRecord LoadRecordDelegate(string id);
        internal event LoadRecordDelegate LoadRecord;

        internal delegate void StoreRecordDelegate(JellyRecord record);
        internal event StoreRecordDelegate StoreRecord;

        public void Dispose()
        {
            
        }
    }
}
