using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using JellyDb.Core.VirtualAddressSpace;

namespace JellyDb.Core.Test.Unit.VirtualAddressSpace
{
    [TestClass]
    public class PageSummaryTest
    {
        private MemoryStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private PageSummary demo;


        [TestInitialize]
        public void TestInitialise()
        {
            stream = new MemoryStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
            demo = new PageSummary();
            demo.Size = 1;
            demo.DataFileOffset = 2;
            demo.Allocated = true;
            demo.AddressSpaceId = Guid.NewGuid();
            demo.Used = 200;
            writer.Write(demo.AddressSpaceId.ToByteArray());
            writer.Write(demo.Size);
            writer.Write(demo.Used);
            writer.Write(demo.DataFileOffset);
            writer.Write(demo.Allocated);
            stream.Position = 0;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            reader = null;
            writer = null;
            stream = null;
        }

        [TestMethod]
        public void PageSummary_SerializeAndDeSerialize()
        {
            long pos = stream.Position;
            demo.WriteToStream(writer);
            Assert.AreEqual(pos, demo.PageFileIndex);
            stream.Position = pos;
            PageSummary target = PageSummary.ReadFromStream(reader);
            Assert.IsNotNull(target);
            Assert.AreEqual(demo.AddressSpaceId, target.AddressSpaceId);
            Assert.AreEqual(demo.Size, target.Size);
            Assert.AreEqual(demo.Used, target.Used);
            Assert.AreEqual(demo.DataFileOffset, target.DataFileOffset);
            Assert.AreEqual(demo.Allocated, target.Allocated);
            Assert.AreEqual(demo.PageFileIndex, target.PageFileIndex);
        }

        [TestMethod]
        public void PageSummary_CreateFromStreamEmpty()
        {
            long pos = stream.Position;
            PageSummary target = PageSummary.ReadFromStream(reader);
            Assert.IsNotNull(target);
            Assert.AreEqual(demo.AddressSpaceId, target.AddressSpaceId);
            Assert.AreEqual(demo.Size, target.Size);
            Assert.AreEqual(demo.Used, target.Used);
            Assert.AreEqual(demo.DataFileOffset, target.DataFileOffset);
            Assert.AreEqual(demo.Allocated, target.Allocated);
            Assert.AreEqual(pos, target.PageFileIndex);
            PageSummary expectedToBeNull = PageSummary.ReadFromStream(reader);
            Assert.IsNull(expectedToBeNull);
        }
    }
}
