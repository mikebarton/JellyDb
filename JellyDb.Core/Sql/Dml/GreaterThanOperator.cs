﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Sql.Dml
{
    public class GreaterThanOperator : ComparisonOperator
    {
        public override Term Process(Term leftOperand, Term rightOperand)
        {
            throw new NotImplementedException();
        }
    }
}
