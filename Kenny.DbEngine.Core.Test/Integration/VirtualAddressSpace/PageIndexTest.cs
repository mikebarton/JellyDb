using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Kenny.DbEngine.Core.VirtualAddressSpace;
using System.IO;

namespace Kenny.DbEngine.Core.Test.Integration.VirtualAddressSpace
{
    [TestClass]
    public class PageIndexTest
    {
        private PageIndex target;

        [TestMethod]
        public void CreateEmptyIndex()
        {
            FileStream stream = File.Create("testIndex.pages");
            target = new PageIndex(stream);

        }
    }
}
