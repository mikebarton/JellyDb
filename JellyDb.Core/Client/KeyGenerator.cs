using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Client
{
    public class KeyGenerator<TKey,TSource> : IKeyGenerator where TSource : class
    {
        private Func<TSource, TKey> _generatorFunction;
        public KeyGenerator(Func<TSource,TKey> generatorFunction)
        {
            _generatorFunction = generatorFunction;
        }

        public object GenerateKey<TEntity>(TEntity entity)
        {
            var typeEntity = typeof(TEntity);
            var typeSource = typeof(TSource);
            if (typeEntity != typeSource) throw new InvalidOperationException(string.Format("Attempting to Generate key for type {0} using generator that expects type {1}", typeEntity.Name, typeSource.Name));

            var key = _generatorFunction(entity as TSource);
            return key;
        }

        public Type GetKeyType()
        {
            return typeof(TKey);
        }
    }
}
