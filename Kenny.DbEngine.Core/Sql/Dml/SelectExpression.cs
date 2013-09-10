using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kenny.DbEngine.Core.Sql.Dml
{
    public class SelectExpression
    {
        private bool SelectAll;
        private List<SelectSome> selectedColumns;
    }
}
