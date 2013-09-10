using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kenny.DbEngine.Core.Sql.Dml
{
    public abstract class Operator
    {
        public abstract Term Process(Term leftOperand, Term rightOperand);        
    }
}
