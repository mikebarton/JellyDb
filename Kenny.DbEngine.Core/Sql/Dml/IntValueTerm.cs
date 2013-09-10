using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kenny.DbEngine.Core.Sql.Dml
{
    public class IntValueTerm : ValueTerm<int>
    {
        public override int Value { get; set; }

    }
}
