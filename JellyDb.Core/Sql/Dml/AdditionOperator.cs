using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Sql.Dml
{
    public class AdditionOperator : ArithmeticOperator
    {
        public override Term Process(Term leftOperand, Term rightOperand)
        {
            if (leftOperand == null || rightOperand == null)
                throw new ArgumentNullException();

            return Solve(Evaluate(leftOperand), Evaluate(rightOperand));
        }

        private Term Solve(Term leftOperand, Term rightOperand)
        {
            ValidateOperand(leftOperand);
            ValidateOperand(rightOperand);
            ValueTerm<dynamic> term = (ValueTerm<dynamic>)leftOperand;
            throw new NotImplementedException();
        }

        
        

        
    }
}
