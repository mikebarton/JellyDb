using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Sql.Dml;

namespace JellyDb.Core.Sql.Dml
{
    public abstract class ArithmeticOperator : Operator
    {
        protected void ValidateOperand(Term operand)
        {
            Type type = operand.GetType();
            if (type != typeof(IntValueTerm)
                && type != typeof(FloatValueTerm)
                && type != typeof(ExpressionTerm))
                throw new SqlParseException("Can only perform arithmetic operations on an int or float");
        }

        protected Term Return(object obj)
        {
            Term result;
            if (obj is float)
            {
                result = new FloatValueTerm() { Value = (float)obj };
            }
            else
            {
                result = new IntValueTerm() { Value = (int)obj };
            }
            return result;
        }

        protected Term Evaluate(Term term)
        {
            ExpressionTerm expressionTerm = term as ExpressionTerm;
            if (expressionTerm != null)
            {
                return expressionTerm.Expression.Evaluate();
            }
            return term;
        }

    }
}
