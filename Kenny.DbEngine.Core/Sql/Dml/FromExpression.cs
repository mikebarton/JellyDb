using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kenny.DbEngine.Core.Sql.Dml
{
    public abstract class FromExpression
    {
        private string newTableAlias;
        private List<Join> joins;
    }
}
