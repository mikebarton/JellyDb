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

namespace JellyDb.Core.Test.Integration.VirtualAddressSpace
{
    [TestClass]
    public class FileDataPersistenceTest
    {
        AddressSpaceManager target;
        private IDataStorage _storage;
        private string fileLoc;

        public FileDataPersistenceTest()
        {
            var folderPath = DbEngineConfigurationSection.ConfigSection.FolderPath;
            fileLoc = Path.Combine(folderPath, "dbFile");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var stream = File.Open(string.Format("{0}.dat", fileLoc), FileMode.OpenOrCreate);
            _storage = new IoFileManager();
            _storage.Initialise(stream);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            target = null;
            _storage = null;
            var dbFile = string.Format("{0}.dat", fileLoc);
            if (File.Exists(dbFile))
            {
                File.Delete(dbFile);
                File.Delete(string.Format("{0}.pages", fileLoc));
            }
        }

        [TestMethod]
        public void FileBasedPersistence_CreateFilePersistence()
        {
            using (target = new AddressSpaceManager(_storage))
            {
                
            }
        }

        [TestMethod]
        public void FileBasedPersistence_SaveAndGetContiguousData()
        {
            using (target = new AddressSpaceManager(_storage))
            {
                Guid id1 = target.CreateVirtualAddressSpace();
                byte[] data = CreateTestByteArray(8, 1024);
                target.SetData(id1, 0, 0, (8 * 1024), data);
                byte[] retrieved = target.GetData(id1, 0, data.Length);
                for (int i = 0; i < data.Length; i++)
                {
                    Assert.AreEqual(data[i], retrieved[i]);
                }
            }
        }

        [TestMethod]
        public void FileBasedPersistence_SaveAndGetScatteredData()
        {
            using (target = new AddressSpaceManager(_storage))
            {
                Guid id1 = target.CreateVirtualAddressSpace();
                Guid id2 = target.CreateVirtualAddressSpace();
                byte[] data = CreateTestByteArray(8, 100);
                target.SetData(id1, 0, 0, 100, data);
                target.SetData(id2, 0, 100, 100, data);
                target.SetData(id1, 100, 200, 100, data);
                target.SetData(id2, 100, 300, 100, data);
                target.SetData(id1, 200, 400, 100, data);
                target.SetData(id2, 200, 500, 100, data);
                target.SetData(id1, 300, 600, 100, data);
                target.SetData(id2, 300, 700, 100, data);
                byte[] retrieved = target.GetData(id1, 0, 4096);
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
            using (target = new AddressSpaceManager(_storage))
            {
                Guid id1 = target.CreateVirtualAddressSpace();
                Guid id2 = target.CreateVirtualAddressSpace();
                byte[] data = CreateTestByteArray(8, 100);
                target.SetData(id1, 0, 0, data.Length, data);
                byte[] retrieved = target.GetData(id1, 0, data.Length);
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
            using (target = new AddressSpaceManager(_storage))
            {
                Guid id1 = target.CreateVirtualAddressSpace();
                sw.Start();
                target.SetData(id1, 0, 0, data.Length, data);
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
