using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.VirtualAddressSpace.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.Engine.Fun;
using JellyDb.Core.Configuration;
using System.IO;

namespace JellyDb.Core.Test.Unit.Engine
{
    [TestClass]
    public class DatabaseTest
    {
        private Database _target = null;
        private string _fileLoc;
        private string _indexLoc;

        public DatabaseTest()
        {
            var folderPath = DbEngineConfigurationSection.ConfigSection.FolderPath;
            _fileLoc = Path.Combine(folderPath, "dbFile.dat");
            _indexLoc = Path.Combine(folderPath, "dbIndex.dat");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            if(File.Exists(_fileLoc))File.Delete(_fileLoc);
            if (File.Exists(_indexLoc)) File.Delete(_indexLoc);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void Database_CreateDatabase()
        {
            using (var indexIo = new IoFileManager(_indexLoc))
            {
                indexIo.Initialise();
                var index = new Index();
                index.WriteToDisk = buffer =>
                {
                    indexIo.WriteData(ref buffer, 0, 0, buffer.Length);
                    return 0;
                };
                using (_target = new Database(index))
                {

                }
            }
        }

        [TestMethod]
        public void Database_CreateAndSaveDatabase()
        {
            using(var indexIo = new IoFileManager(_indexLoc))
            {
                indexIo.Initialise();
                var index = new Index();
                index.WriteToDisk = buffer =>
                {
                    indexIo.WriteData(ref buffer, 0, 0, buffer.Length);
                    return 0;
                };

                using (var fileIo = new IoFileManager(_fileLoc))
                using (_target = new Database(index))
                {
                    fileIo.Initialise();
                    _target.ReadFromDisk = (offset, bytes) =>
                    {
                        var buffer = new byte[bytes];
                        fileIo.ReadData(ref buffer, 0, offset, bytes);
                        return buffer;
                    };

                    _target.WriteToDisk = buffer =>
                    {
                        var offset = fileIo.EndOfStreamIndex;
                        fileIo.WriteData(ref buffer, 0, offset, buffer.Length);
                        return offset;
                    };


                    _target.Write(123, "hello monkey");
                    _target.Write(456, "how are you?");
                    Assert.AreEqual("hello monkey", _target.Read(123));
                    Assert.AreEqual("how are you?", _target.Read(456));
                }
            }            
        }

        [TestMethod]
        public void Database_CreateAndSaveLOTSInDatabase()
        {
            using (var indexIo = new IoFileManager(_indexLoc))
            {
                indexIo.Initialise();
                var index = new Index();
                index.WriteToDisk = buffer =>
                {
                    indexIo.WriteData(ref buffer, 0, 0, buffer.Length);
                    return 0;
                };
                using (var fileIo = new IoFileManager(_fileLoc))
                using (_target = new Database(index))
                {
                    fileIo.Initialise();
                    _target.ReadFromDisk = (offset, bytes) =>
                    {
                        var buffer = new byte[bytes];
                        fileIo.ReadData(ref buffer, 0, offset, bytes);
                        return buffer;
                    };

                    _target.WriteToDisk = buffer =>
                    {
                        var offset = fileIo.EndOfStreamIndex;
                        fileIo.WriteData(ref buffer, 0, offset, buffer.Length);
                        return offset;
                    };


                    for (int i = 1; i < 1000; i++)
                    {
                        _target.Write(i, string.Format("hello {0}", i));
                    }
                    fileIo.Flush();
                    for (int i = 1; i < 1000; i++)
                    {
                        Assert.AreEqual(string.Format("hello {0}", i), _target.Read(i));
                    }
                }
            }
        }

        [TestMethod]
        public void Database_CreateCloseAndReadDatabase()
        {
            using (var indexIo = new IoFileManager(_indexLoc))
            {
                indexIo.Initialise();
                var index = new Index();
                index.WriteToDisk = buffer =>
                {
                    indexIo.WriteData(ref buffer, 0, 0, buffer.Length);
                    return 0;
                };
                using (var fileIo = new IoFileManager(_fileLoc))
                using (_target = new Database(index))
                {
                    fileIo.Initialise();
                    _target.ReadFromDisk = (offset, bytes) =>
                    {
                        var buffer = new byte[bytes];
                        fileIo.ReadData(ref buffer, 0, offset, bytes);
                        return buffer;
                    };

                    _target.WriteToDisk = buffer =>
                    {
                        var offset = fileIo.EndOfStreamIndex;
                        fileIo.WriteData(ref buffer, 0, offset, buffer.Length);
                        return offset;
                    };


                    for (int i = 1; i < 10; i++)
                    {
                        _target.Write(i, string.Format("hello {0}", i));
                    }
                }
            }

            using (var indexIo = new IoFileManager(_indexLoc))
            {
                indexIo.Initialise();
                var index = Index.Load(indexIo.ReadToEnd(0));
                index.WriteToDisk = buffer =>
                {
                    indexIo.WriteData(ref buffer, 0, 0, buffer.Length);
                    return 0;
                };
                using (var fileIo = new IoFileManager(_fileLoc))
                using (_target = new Database(index))
                {
                    fileIo.Initialise();
                    _target.ReadFromDisk = (offset, bytes) =>
                    {
                        var buffer = new byte[bytes];
                        fileIo.ReadData(ref buffer, 0, offset, bytes);
                        return buffer;
                    };

                    _target.WriteToDisk = buffer =>
                    {
                        var offset = fileIo.EndOfStreamIndex;
                        fileIo.WriteData(ref buffer, 0, offset, buffer.Length);
                        return offset;
                    };


                    for (int i = 1; i < 10; i++)
                    {
                        Assert.AreEqual(string.Format("hello {0}", i), _target.Read(i));
                    }
                }
            }
        }
    }
}
