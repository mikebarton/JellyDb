using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.Engine.Fun;

namespace JellyDb.Core.Test.Unit.Engine
{
    [TestClass]
    public class DatabaseTest
    {
        private Database _target = null;

        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void Database_CreateDatabase()
        {
            using (_target = new Database("testDatabase"))
            {

            }
        }
    }
}
