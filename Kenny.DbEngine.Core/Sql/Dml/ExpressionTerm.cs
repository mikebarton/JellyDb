using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kenny.DbEngine.Core.Sql.Dml
{
    public class ExpressionTerm : Term
    {
        private Expression expression;

        public Expression Expression
        {
            get
            {
                return expression;
            }
            set
            {
                expression = value;
            }
        }
    }
}
