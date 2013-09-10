using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kenny.DbEngine.Core.Sql.Dml
{
    public class ColumnNameTerm : Term
    {        
        public string TableName { get; set; }
        public string ColumnName { get; set; }

    }
}
