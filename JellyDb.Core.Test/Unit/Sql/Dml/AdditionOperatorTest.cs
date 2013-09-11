using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.Sql.Dml;

namespace JellyDb.Core.Test.Unit.Sql.Dml
{
    [TestClass]
    public class AdditionOperatorTest
    {
        private AdditionOperator target;

        public AdditionOperatorTest()
        {
            target = new AdditionOperator();
        }

        [TestMethod]
        [ExpectedException(typeof(SqlParseException))]
        public void ProcessColumnNameTerms()
        {
            ColumnNameTerm left = new ColumnNameTerm() { TableName = "table", ColumnName = "column" };
            IntValueTerm right = new IntValueTerm() { Value = 5 };
            target.Process(left, right);
        }

        [TestMethod]
        public void ProcessValueTerms()
        {
        }

        [TestMethod]
        public void ProcessNullTerms()
        {
        }

        [TestMethod]
        public void ProcessExpressionTerms()
        {
        }

        [TestMethod]
        public void ProcessBooleanTerms()
        {
        }

        [TestMethod]
        public void ProcessStringTerms()
        {
        }

        [TestMethod]
        public void ProcessDateTimeTerms()
        {
        }

        [TestMethod]
        public void ProcessIntTerms()
        {
        }

        [TestMethod]
        public void ProcessByteArrayTerms()
        {
        }

    }
}
