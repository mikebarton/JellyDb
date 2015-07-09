using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class AutoGenIdentity<TKey>
    {
        private readonly ITypeComparer<TKey> _typeComparer;
        public AutoGenIdentity()
        {
            _typeComparer = TypeComparer<TKey>.GetTypeComparer();
        }

        public TKey GetNextId()
        {
            CurrentUsedId = _typeComparer.Increment(CurrentUsedId);
            var result = CurrentUsedId;
            NextRetrieved(this);
            return result;
        }

        public TKey CurrentUsedId { get; set; }

        public event GetNextIdentity<TKey> NextRetrieved;
        public delegate void GetNextIdentity<TK>(AutoGenIdentity<TK> sender);

        public string EntityTypeName { get; set; }
    }
}
