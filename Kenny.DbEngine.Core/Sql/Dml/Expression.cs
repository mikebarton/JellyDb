using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kenny.DbEngine.Core.Sql.Dml
{
    public class Expression
    {
        private Term leftOperand;
        private Term rightOperand;
        private Operator @operator;

        public Term Evaluate()
        {
            throw new NotImplementedException();
        }
    }
}
