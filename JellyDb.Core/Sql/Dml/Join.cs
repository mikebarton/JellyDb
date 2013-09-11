using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Sql.Dml
{
    public abstract class Join
    {
        private Expression onExpression;
        private FromExpression joinDestination;
    }
}
