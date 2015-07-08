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
            NextIdentityRetrieved(this, EventArgs.Empty);
            return result;
        }

        public TKey CurrentUsedId { get; set; }

        public event EventHandler NextIdentityRetrieved; 

        public string EntityTypeName { get; set; }
    }
}
