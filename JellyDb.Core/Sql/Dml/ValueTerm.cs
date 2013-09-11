using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Sql.Dml
{
    public abstract class ValueTerm<T> : Term
    {
        public abstract T Value { get; set; }        
    }
}
