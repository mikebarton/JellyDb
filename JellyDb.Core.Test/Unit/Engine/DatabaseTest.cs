using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.VirtualAddressSpace.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.Engine.Fun;
using JellyDb.Core.Configuration;

namespace JellyDb.Core.Test.Unit.Engine
{
    [TestClass]
    public class DatabaseTest
    {
        private Database _target = null;

        [TestInitialize]
        public void TestInitialize()
        {
            
        }

        [TestCleanup]
        public void TestCleanup()
        {

        }

        [TestMethod]
        public void Database_CreateDatabase()
        {
            using (_target = new Database("testDatabase"))
            {

            }
        }

        [TestMethod]
        public void Database_CreateAndSaveDatabase()
        {
            using (var fileIo = new IoFileManager())
            using (_target = new Database("testDatabase"))
            {
                fileIo.Initialise();
                _target.ReadFromDisk = (offset, bytes) =>
                    {
                        var buffer = new byte[bytes];
                        fileIo.ReadVirtualPage(ref buffer,0,offset,bytes);
                        return buffer;
                    };

                _target.WriteToDisk = buffer =>
                    {
                        var offset = fileIo.EndOfStreamIndex;
                        fileIo.WriteVirtualPage(ref buffer, 0, offset, buffer.Length);
                        return offset;
                    };


                _target.Write(123, "hello monkey");
                _target.Write(456, "how are you?");
                Assert.AreEqual("hello monkey", _target.Read(123));
                Assert.AreEqual("how are you?", _target.Read(456));
            }
        }

        [TestMethod]
        public void Database_CreateAndSaveLOTSInDatabase()
        {
            using (var fileIo = new IoFileManager())
            using (_target = new Database("testDatabase"))
            {
                fileIo.Initialise();
                _target.ReadFromDisk = (offset, bytes) =>
                {
                    var buffer = new byte[bytes];
                    fileIo.ReadVirtualPage(ref buffer, 0, offset, bytes);
                    return buffer;
                };

                _target.WriteToDisk = buffer =>
                {
                    var offset = fileIo.EndOfStreamIndex;
                    fileIo.WriteVirtualPage(ref buffer, 0, offset, buffer.Length);
                    return offset;
                };


                for (int i = 1; i < 100000; i++)
                {
                    _target.Write(i, string.Format("hello {0}", i));
                }
                fileIo.Flush();
                for (int i = 1; i < 100000; i++)
                {
                    Assert.AreEqual(string.Format("hello {0}", i), _target.Read(i));
                }
            }
        }
    }
}
