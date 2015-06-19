using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.VirtualAddressSpace.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.Engine.Fun;
using JellyDb.Core.Configuration;
using System.IO;
using JellyDb.Core.Storage;

namespace JellyDb.Core.Test.Unit.Engine
{
    [TestClass]
    public class DatabaseTest
    {
        private Database _target = null;
        private IDataStorage _dataStorage;
        private IDataStorage _indexStorage;

        public DatabaseTest()
        {
        
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _dataStorage = new MemoryStreamManager();
            _dataStorage.Initialise();
            _indexStorage = new MemoryStreamManager();
            _indexStorage.Initialise();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _target.Dispose();
            _target = null;
            _dataStorage.Dispose();
            _dataStorage = null;
            _indexStorage.Dispose();
            _indexStorage = null;
            
        }

        [TestMethod]
        public void Database_CreateDatabase()
        {
            var index = new Index(_indexStorage);
                
            using (_target = new Database(index, _dataStorage))
            {

            }            
        }

        [TestMethod]
        public void Database_CreateAndSaveDatabase()
        {
            var index = new Index(_indexStorage);                

            using (_target = new Database(index, _dataStorage))
            {
                _target.Write(123, "hello monkey");
                _target.Write(456, "how are you?");
                Assert.AreEqual("hello monkey", _target.Read(123));
                Assert.AreEqual("how are you?", _target.Read(456));
            }                        
        }

        [TestMethod]
        public void Database_CreateAndSaveLOTSInDatabase()
        {
            var index = new Index(_indexStorage);
            using (_target = new Database(index, _dataStorage))
            {                    
                for (int i = 1; i < 1000; i++)
                {
                    _target.Write(i, string.Format("hello {0}", i));
                }
                for (int i = 1; i < 1000; i++)
                {
                    Assert.AreEqual(string.Format("hello {0}", i), _target.Read(i));
                }
            }            
        }
    }
}
