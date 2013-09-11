using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Sql.Dml
{
    class Select : Term
    {
        private int TopCount;
        private SetQuantifier Quantifier;
        private SelectExpression SelectExpression;
        private FromExpression FromExpression;
        private Expression WhereExpression;
    }
}
