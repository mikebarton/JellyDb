using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using JellyDb.Core.Configuration;

namespace JellyDb.Core.Test.Unit.Configuration
{
    [TestClass]
    public class VirtualFileSystemConfigurationTest
    {
        [TestMethod]
        public void GetConfigurationSection()
        {
            VirtualFileSystemConfigurationSection target = VirtualFileSystemConfigurationSection.ConfigSection;
            Assert.IsNotNull(target);
            Assert.AreEqual(65536, target.PageSizeInKb);
            Assert.AreEqual(4, target.PageIncreaseNum);
            Assert.AreEqual(@"c:\Temp\DbEngine.db", target.vfsFileName);
        }
    }
}
