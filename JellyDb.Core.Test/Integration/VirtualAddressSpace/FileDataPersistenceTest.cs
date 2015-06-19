using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using JellyDb.Core.VirtualAddressSpace.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.VirtualAddressSpace;
using JellyDb.Core.Configuration;
using System.IO;
using System.Diagnostics;
using JellyDb.Core.Storage;

namespace JellyDb.Core.Test.Integration.VirtualAddressSpace
{
    [TestClass]
    public class FileDataPersistenceTest
    {
        AddressSpaceManager target;
        private IDataStorage _dataStorage;

        public FileDataPersistenceTest()
        {       
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _dataStorage = new MemoryStreamManager();
            _dataStorage.Initialise();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            target.Dispose();
            target = null; 
            _dataStorage.Dispose();
            _dataStorage = null;            
        }

        [TestMethod]
        public void FileBasedPersistence_CreateFilePersistence()
        {
            using (target = new AddressSpaceManager(_dataStorage))
            {
                
            }
        }

        [TestMethod]
        public void FileBasedPersistence_SaveAndGetContiguousData()
        {
            using (target = new AddressSpaceManager(_dataStorage))
            {
                var id1 = Guid.NewGuid();
                var agent = target.CreateVirtualAddressSpaceAgent(id1);
                byte[] data = CreateTestByteArray(8, 1024);
                agent.WriteData(ref data, 0, 0, (8 * 1024));
                byte[] retrieved = new byte[data.Length];
                agent.ReadData(ref retrieved, 0, 0, retrieved.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    Assert.AreEqual(data[i], retrieved[i]);
                }
            }
        }

        [TestMethod]
        public void FileBasedPersistence_SaveAndGetScatteredData()
        {
            using (target = new AddressSpaceManager(_dataStorage))
            {
                Guid id1 = Guid.NewGuid();
                Guid id2 = Guid.NewGuid();
                var agent1 = target.CreateVirtualAddressSpaceAgent(id1);
                var agent2 = target.CreateVirtualAddressSpaceAgent(id2);
                byte[] data = CreateTestByteArray(8, 100);

                agent1.WriteData(ref data, 0, 0, 100);
                agent2.WriteData(ref data, 100, 0, 100);
                agent1.WriteData(ref data, 200, 100, 100);
                agent2.WriteData(ref data, 300, 100, 100);
                agent1.WriteData(ref data, 400, 200, 100);
                agent2.WriteData(ref data, 500, 200, 100);
                agent1.WriteData(ref data, 600, 300, 100);
                agent2.WriteData(ref data, 700, 300, 100);
                //agent1.SetData(id1, 0, 0, 100, data);
                //agent2.SetData(id2, 0, 100, 100, data);
                //agent1.SetData(id1, 100, 200, 100, data);
                //agent2.SetData(id2, 100, 300, 100, data);
                //agent1.SetData(id1, 200, 400, 100, data);
                //agent2.SetData(id2, 200, 500, 100, data);
                //agent1.SetData(id1, 300, 600, 100, data);
                //agent2.SetData(id2, 300, 700, 100, data);
                byte[] retrieved = new byte[4096];
                agent1.ReadData(ref retrieved, 0, 0, 4096);
                int count = -2;
                for (int i = 0; i < retrieved.Length; i++)
                {
                    if (i < 400)
                    {
                        if (i % 100 == 0) count += 2;
                        Assert.AreEqual(count, retrieved[i]);
                    }
                    else
                    {
                        Assert.AreEqual(0, retrieved[i]);
                    }
                }
            }
        }

        [TestMethod]
        public void FileBasedPersitence_SaveMultipleAddressSpacesWithOneFullOfdata()
        {
            using (target = new AddressSpaceManager(_dataStorage))
            {
                Guid id1 = Guid.NewGuid();
                Guid id2 = Guid.NewGuid();
                var agent1 = target.CreateVirtualAddressSpaceAgent(id1);
                var agent2 = target.CreateVirtualAddressSpaceAgent(id2);
                byte[] data = CreateTestByteArray(8, 100);
                agent1.WriteData(ref data, 0, 0, data.Length);
                var retrieved = new byte[data.Length];
                agent1.ReadData(ref retrieved, 0, 0, retrieved.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    Assert.AreEqual(data[i], retrieved[i]);
                }
            }
        }

        [TestMethod]
        public void CreateBigFileTest()
        {
            Stopwatch sw = new Stopwatch();
            byte[] data = CreateTestByteArray((1024 * 1024), 5);
            using (target = new AddressSpaceManager(_dataStorage))
            {
                Guid id1 = Guid.NewGuid();
                var agent1 = target.CreateVirtualAddressSpaceAgent(id1);
                sw.Start();
                agent1.WriteData(ref data, 0, 0, data.Length);
                sw.Stop();
            }
        }

        private byte[] CreateTestByteArray(int numPages, int pageSize)
        {
            int length = numPages * pageSize;
            byte[] data = new byte[length];
            for (int i = 0; i < numPages; i++)
            {
                for (int j = 0; j < pageSize; j++)
                {
                    int level = i * pageSize;
                    int index = level + j;
                    data[index] = (byte)i;
                }
            }
            return data;
        }

    }
}
