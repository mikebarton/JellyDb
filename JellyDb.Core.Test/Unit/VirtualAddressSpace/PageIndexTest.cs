using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.VirtualAddressSpace;
using System.IO;
using System.Runtime.InteropServices;

namespace JellyDb.Core.Test.Unit.VirtualAddressSpace
{
    [TestClass]
    public class PageIndexTest
    {
        private PageIndex target;
        private MemoryStream stream;        
        Guid id1 = Guid.NewGuid();
        Guid id2 = Guid.NewGuid();

        [TestInitialize]
        public void TestInitialize()
        {
            stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            PageSummary summary1 = new PageSummary() { AddressSpaceId = id1, Allocated = true, Offset = 0, Size = 1024, Used = 1024 };
            PageSummary summary2 = new PageSummary() { AddressSpaceId = id2, Allocated = true, Offset = 1024, Size = 1024, Used = 100 };
            PageSummary summary3 = new PageSummary() { AddressSpaceId = id1, Allocated = true, Offset = 2048, Size = 1024, Used = 200 };
            PageSummary summary4 = new PageSummary() { AddressSpaceId = Guid.Empty, Allocated = false, Offset = 3072, Size = 1024, Used = 0 };
            summary1.WriteToStream(writer);
            summary2.WriteToStream(writer);
            summary3.WriteToStream(writer);
            summary4.WriteToStream(writer);
            stream.Position = 0;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            stream.Close();
            stream = null;
        }

        [TestMethod]
        public void PageIndex_CreateEmptyPageIndex()
        {
            var otherStream = new MemoryStream();
            target = new PageIndex(otherStream);            
        }

        [TestMethod]
        public void PageIndex_OpenExistingPageIndex()
        {
            using (target = new PageIndex(stream))
            {
                Assert.IsNotNull(target[id1]);
                Assert.IsNotNull(target[id2]);
                Assert.AreEqual(2, target[id1].Count());
                Assert.AreEqual(1, target[id2].Count());
                Assert.AreEqual(1, target[Guid.Empty].Count());

                Assert.AreEqual(0, target[id1][0].Offset);
                Assert.AreEqual(1024, target[id1][0].Size);
                Assert.AreEqual(1024, target[id1][0].Used);
                Assert.IsTrue(target[id1][0].Allocated);
                Assert.AreEqual(0, target[id1][0].PageFileIndex);
                Assert.AreEqual(id1, target[id1][0].AddressSpaceId);

                Assert.AreEqual(2048, target[id1][1].Offset);
                Assert.AreEqual(1024, target[id1][1].Size);
                Assert.AreEqual(200, target[id1][1].Used);
                Assert.IsTrue(target[id1][1].Allocated);
                Assert.AreEqual(66, target[id1][1].PageFileIndex);
                Assert.AreEqual(id1, target[id1][1].AddressSpaceId);

                Assert.AreEqual(1024, target[id2][0].Offset);
                Assert.AreEqual(1024, target[id2][0].Size);
                Assert.AreEqual(100, target[id2][0].Used);
                Assert.IsTrue(target[id2][0].Allocated);
                Assert.AreEqual(33, target[id2][0].PageFileIndex);
                Assert.AreEqual(id2, target[id2][0].AddressSpaceId);

                Assert.AreEqual(3072, target[Guid.Empty][0].Offset);
                Assert.AreEqual(1024, target[Guid.Empty][0].Size);
                Assert.AreEqual(0, target[Guid.Empty][0].Used);
                Assert.IsFalse(target[Guid.Empty][0].Allocated);
                Assert.AreEqual(99, target[Guid.Empty][0].PageFileIndex);
                Assert.AreEqual(Guid.Empty, target[Guid.Empty][0].AddressSpaceId);

                Assert.AreEqual(4096, target.EndOfPageIndex);
            }
        }

        [TestMethod]
        public void PageIndex_CreatePageIndexWithEntries()
        {
            using (target = new PageIndex(stream))
            {
                //PageSummary summary1 = new PageSummary() { AddressSpaceId = id1, Allocated = true, Offset = 0, Size = 1024, Used = 1024 };
                //PageSummary summary2 = new PageSummary() { AddressSpaceId = id2, Allocated = true, Offset = 1024, Size = 1024, Used = 100 };
                //PageSummary summary3 = new PageSummary() { AddressSpaceId = id1, Allocated = true, Offset = 2048, Size = 1024, Used = 200 };
                //PageSummary summary4 = new PageSummary() { AddressSpaceId = Guid.Empty, Allocated = false, Offset = 3072, Size = 1024, Used = 0 };

                //target.AddOrUpdateEntry(summary1);
                //target.AddOrUpdateEntry(summary2);
                //target.AddOrUpdateEntry(summary3);
                //target.AddOrUpdateEntry(summary4);

                Assert.IsNotNull(target[id1]);
                Assert.IsNotNull(target[id2]);
                Assert.AreEqual(2, target[id1].Count());
                Assert.AreEqual(1, target[id2].Count());
                Assert.AreEqual(1, target[Guid.Empty].Count());

                Assert.AreEqual(0, target[id1][0].Offset);
                Assert.AreEqual(1024, target[id1][0].Size);
                Assert.AreEqual(1024, target[id1][0].Used);
                Assert.IsTrue(target[id1][0].Allocated);
                Assert.AreEqual(0, target[id1][0].PageFileIndex);
                Assert.AreEqual(id1, target[id1][0].AddressSpaceId);

                Assert.AreEqual(2048, target[id1][1].Offset);
                Assert.AreEqual(1024, target[id1][1].Size);
                Assert.AreEqual(200, target[id1][1].Used);
                Assert.IsTrue(target[id1][1].Allocated);
                Assert.AreEqual(66, target[id1][1].PageFileIndex);
                Assert.AreEqual(id1, target[id1][1].AddressSpaceId);

                Assert.AreEqual(1024, target[id2][0].Offset);
                Assert.AreEqual(1024, target[id2][0].Size);
                Assert.AreEqual(100, target[id2][0].Used);
                Assert.IsTrue(target[id2][0].Allocated);
                Assert.AreEqual(33, target[id2][0].PageFileIndex);
                Assert.AreEqual(id2, target[id2][0].AddressSpaceId);

                Assert.AreEqual(3072, target[Guid.Empty][0].Offset);
                Assert.AreEqual(1024, target[Guid.Empty][0].Size);
                Assert.AreEqual(0, target[Guid.Empty][0].Used);
                Assert.IsFalse(target[Guid.Empty][0].Allocated);
                Assert.AreEqual(99, target[Guid.Empty][0].PageFileIndex);
                Assert.AreEqual(Guid.Empty, target[Guid.Empty][0].AddressSpaceId);
                Assert.AreEqual(4096, target.EndOfPageIndex);
            }

        }

        [TestMethod]
        public void PageIndex_AddEntryToEmptyPageIndex()
        {
            stream = new MemoryStream();
            using (target = new PageIndex(stream))
            {
                PageSummary summary1 = new PageSummary() { AddressSpaceId = id1, Allocated = true, Offset = 0, Size = 1024, Used = 1024 };
                target.AddOrUpdateEntry(summary1);
                Assert.AreEqual(0, target[id1][0].Offset);
                Assert.AreEqual(1024, target[id1][0].Size);
                Assert.AreEqual(1024, target[id1][0].Used);
                Assert.IsTrue(target[id1][0].Allocated);
                Assert.AreEqual(0, target[id1][0].PageFileIndex);
                Assert.AreEqual(id1, target[id1][0].AddressSpaceId);
                Assert.AreEqual(1024, target.EndOfPageIndex);
            }
        }

        [TestMethod]
        public void PageIndex_AddEntryToPageIndexWithEntries()
        {
            using (target = new PageIndex(stream))
            {
                PageSummary summary1 = new PageSummary() { Allocated = true, Offset = 4096, Size = 1024, AddressSpaceId=id1, Used = 300 };
                target.AddOrUpdateEntry(summary1);
                Assert.AreEqual(3, target[id1].Count());
                Assert.AreEqual(4096, target[id1][2].Offset);
                Assert.AreEqual(1024, target[id1][2].Size);
                Assert.IsTrue(target[id1][2].Allocated);
                Assert.AreEqual(132, target[id1][2].PageFileIndex);
                Assert.AreEqual(5120, target.EndOfPageIndex);
            }
        }

        [TestMethod]
        public void PageIndex_UpdateEntryInPageIndex()
        {
            using (target = new PageIndex(stream))
            {
                Assert.IsNotNull(target);
                var summary = target[id1][1];
                summary.Used = 400;
                target.AddOrUpdateEntry(summary);
            
                Assert.AreEqual(2, target[id1].Count);
                Assert.AreEqual(0, target[id1][0].Offset);
                Assert.AreEqual(1024, target[id1][0].Size);
                Assert.AreEqual(1024, target[id1][0].Used);
                Assert.IsTrue(target[id1][0].Allocated);
                Assert.AreEqual(0, target[id1][0].PageFileIndex);

                Assert.AreEqual(1, target[id2].Count);
                Assert.AreEqual(1024, target[id2][0].Offset);
                Assert.AreEqual(1024, target[id2][0].Size);
                Assert.AreEqual(100, target[id2][0].Used);
                Assert.IsTrue(target[id2][0].Allocated);
                Assert.AreEqual(33, target[id2][0].PageFileIndex);

                Assert.AreEqual(2048, target[id1][1].Offset);
                Assert.AreEqual(1024, target[id1][1].Size);
                Assert.AreEqual(400, target[id1][1].Used);
                Assert.IsTrue(target[id1][1].Allocated);
                Assert.AreEqual(66, target[id1][1].PageFileIndex);

                Assert.AreEqual(3072, target[Guid.Empty][0].Offset);
                Assert.AreEqual(1024, target[Guid.Empty][0].Size);
                Assert.AreEqual(0, target[Guid.Empty][0].Used);
                Assert.IsFalse(target[Guid.Empty][0].Allocated);
                Assert.AreEqual(99, target[Guid.Empty][0].PageFileIndex);

                Assert.AreEqual(4096, target.EndOfPageIndex);
            }
        }

        [TestMethod]
        public void PageIndex_MoveSummaryFromEmptyToUsed()
        {
            target = new PageIndex(stream);            
            Assert.AreEqual(2, target[id1].Count);
            Assert.AreEqual(1, target.EmptyPages.Count);
            PageSummary summary = target.EmptyPages[0];
            Assert.AreEqual(99, summary.PageFileIndex);
            summary.AddressSpaceId = id1;
            target.AddOrUpdateEntry(summary);
            
            stream.Position = 0;

            var newIndex = new PageIndex(stream);
            Assert.AreEqual(3, target[id1].Count);
            Assert.AreEqual(0, target.EmptyPages.Count);
            PageSummary retrieved = target[id1][2];
            Assert.AreEqual(99, retrieved.PageFileIndex);
        }

        [TestMethod]
        public void PageIndex_MoveSummaryFromEmptyToNew()
        {
            Guid newId = Guid.NewGuid();
            target = new PageIndex(stream);
            Assert.AreEqual(2, target[id1].Count);
            Assert.AreEqual(1, target.EmptyPages.Count);
            PageSummary summary = target.EmptyPages[0];
            Assert.AreEqual(99, summary.PageFileIndex);
            summary.AddressSpaceId = newId;
            target.AddOrUpdateEntry(summary);

            stream.Position = 0;

            var newIndex = new PageIndex(stream);
            Assert.AreEqual(2, target[id1].Count);
            Assert.AreEqual(1, target[newId].Count);
            Assert.AreEqual(0, target.EmptyPages.Count);
            PageSummary retrieved = target[newId][0];
            Assert.AreEqual(99, retrieved.PageFileIndex);
        }

    }
}
