using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.Configuration;
using System.Configuration;
using System.Runtime.InteropServices;
using JellyDb.Core.VirtualAddressSpace;

namespace JellyDb.Core.Test.Unit.Configuration
{
    [TestClass]
    public class DbEngineConfigurationSectionTest
    {
        [TestMethod]
        public void GetConfigurationSection()
        {
            var section = DbEngineConfigurationSection.ConfigSection;
            var folderPath = section.FolderPath;
            var pageSizeIncrease = section.VfsConfig.PageSizeInKb;
            var pageNumIncrease = section.VfsConfig.PageIncreaseNum;
            Assert.AreEqual(@"c:\temp", folderPath);
            Assert.AreEqual(65536, pageSizeIncrease);
            Assert.AreEqual(4, pageNumIncrease);
        }
    }
}
