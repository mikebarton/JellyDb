using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Sql.Dml
{
    public class SqlParseException : Exception
    {
        public SqlParseException(string message) : base(message)
        {

        }
    }
}
