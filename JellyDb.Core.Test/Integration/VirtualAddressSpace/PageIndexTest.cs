using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.VirtualAddressSpace;
using System.IO;

namespace JellyDb.Core.Test.Integration.VirtualAddressSpace
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
