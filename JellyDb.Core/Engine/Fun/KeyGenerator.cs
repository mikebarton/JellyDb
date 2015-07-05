using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class KeyGenerator<TKey,TSource> : IKeyGenerator where TSource : class
    {
        private Action<TSource, TKey> _propertySetter;
        private Func<TSource, TKey> _propertyGetter;
        private bool _autoGenerateKey;

        public KeyGenerator(Expression<Func<TSource,TKey>> propertyExpression, bool autoGenerate)
        {
            _autoGenerateKey = autoGenerate;
            _propertyGetter = propertyExpression.Compile();
            _propertySetter = MakeSetter<TSource, TKey>(propertyExpression).Compile();
        }

        public DataKey GenerateKey(TSource entity)
        {   
            if (_autoGenerateKey) 
            {
                //TODO retrieve auto generated key, store on entity, and return
                return DataKey.CreateKey<TKey>(null);
            }
            else
            {
                var key = _propertyGetter(entity);
                //TODO ensure value is initialised
                return DataKey.CreateKey<TKey>(key);
            }
        }

        public Type GetKeyType()
        {
            return typeof(TKey);
        }

        private static Expression<Action<TSource, TKey>> MakeSetter<T, TProperty>(Expression<Func<T, TProperty>> getter)
        {
            var memberExpr = (MemberExpression)getter.Body;
            var @this = Expression.Parameter(typeof(TSource), "$this");
            var value = Expression.Parameter(typeof(TKey), "value");
            return Expression.Lambda<Action<TSource, TKey>>(
                Expression.Assign(Expression.MakeMemberAccess(@this, memberExpr.Member), value),
                @this, value);
        }
    }
}
