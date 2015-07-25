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
        private AutoGenIdentity<TKey> _autoGenerator;
        

        public KeyGenerator(Expression<Func<TSource,TKey>> propertyExpression, bool autoGenerate)
        {
            _autoGenerateKey = autoGenerate;
            if(propertyExpression == null && !autoGenerate) throw new InvalidOperationException("Cannot create a KeyGenerator that is not auto-generate, but that has not property express");
            
            if(propertyExpression != null)
            {
                _propertyGetter = propertyExpression.Compile();
                _propertySetter = MakeSetter<TSource, TKey>(propertyExpression).Compile();
            }
        }

        public void RegisterAutoGenIdentity(object autoGenerator)
        {
            _autoGenerator = (AutoGenIdentity<TKey>)autoGenerator;
        }

        public DataKey GenerateKey(object entity)
        {   
            if (_autoGenerateKey) 
            {
                //TODO retrieve auto generated key, store on entity, and return
                var key = _autoGenerator.GetNextId();
                var dataKey = DataKey.CreateKey<TKey>(key);
                if(_propertySetter != null) _propertySetter((TSource) entity, dataKey.GetKey<TKey>());
                return dataKey;
            }
            else
            {
                var key = _propertyGetter((TSource)entity);
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
