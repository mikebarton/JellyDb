using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kenny.DbEngine.Core.Sql.Dml
{
    public class NotOperator : LogicalOperator
    {
        public override Term Process(Term leftOperand, Term rightOperand)
        {
            throw new NotImplementedException();
        }
    }
}
