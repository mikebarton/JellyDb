using JellyDb.Core.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class RecordIdentityService
    {
        private Dictionary<Type, IKeyGenerator> _keyGenerators = new Dictionary<Type, IKeyGenerator>();

        public void RegisterTypeIdentity<TSource, TKey>(Expression<Func<TSource, TKey>> propertyExpression, bool autoGenerate) where TSource : class
        {
            ValidateKeyType<TKey>();

            var entityType = typeof(TSource);
            if (_keyGenerators.ContainsKey(entityType))
                throw new InvalidOperationException(string.Format("There is already an identity function registered for for type {0}", entityType.FullName));
            var generator = new KeyGenerator<TKey,TSource>(propertyExpression, autoGenerate);
            _keyGenerators.Add(entityType, generator);
        }

        public DataKey LoadIdentity<TEntity>(JellyRecord<TEntity> record)
        {
            var entityType = typeof(TEntity);
            var generator = _keyGenerators[entityType];
            var dataKey = generator.GenerateKey(record.Entity);
            return dataKey;
        }

        private void ValidateKeyType<TKey>()
        {
            var t = typeof(TKey);
            if (t != typeof(short) &&
                t != typeof(int) &&
                t != typeof(long) &&
                t != typeof(string) &&
                t != typeof(DateTime))
                throw new InvalidOperationException(string.Format("Key type for id property of entity to be stored must be short, int, long,, string or datetime. Not {0}", t.Name));
        }
    }
}
