using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.Client;

namespace JellyDb.Core.Test.Integration.Client
{
    /// <summary>
    /// Summary description for JellyDatabaseTest
    /// </summary>
    [TestClass]
    public class JellyDatabaseTest
    {
        private const string _connectionString = @"c:\temp\jelly\jelly.db";
        private JellyDatabase _db;
        [TestInitialize]
        public void TestInit()
        {

        }

        [TestCleanup]
        public void TestCleanup()
        {
            _db.Dispose();
        }

        [TestMethod]
        public void CreateDataBase()
        {
            var db = new JellyDatabase(_connectionString);            
        }

        [TestMethod]
        public void CreateDataBaseAndRegisterIdentityGenerator()
        {
            var db = new JellyDatabase(_connectionString);            
            db.RegisterIdentityProperty<TestEntity, uint>(e => e.Id, true);            
        }        

        [TestMethod]
        public void CreateDatabaseClientSaveAndRetrieve()
        {
            uint key;
            _db = new JellyDatabase(_connectionString);
            using(var session = _db.CreateSession())
            {
                _db.RegisterIdentityProperty<TestEntity, uint>(entity => entity.Id, true);
                var created = TestEntity.CreateTestEntity(2);
                session.Store(created);
                key = created.Id;
            }            

            using (JellyDatabase db = new JellyDatabase(@"c:\temp\jelly\jelly.db"))
            using (var session = db.CreateSession())
            {
                db.RegisterIdentityProperty<TestEntity, uint>(entity => entity.Id, true);
                var retrieved = session.Load<uint, TestEntity>(key);
            }
        }

        [TestMethod]
        public void CreateManyRecordsAndRetrieveWithAutoGen()
        {
            var keys = new List<uint>();
            _db = new JellyDatabase(_connectionString);
            _db.RegisterIdentityProperty<TestEntity, uint>(e => e.Id, true);
            for (int i = 0; i < 50; i++)
            {
                using (var session = _db.CreateSession())
                {
                    var entity = TestEntity.CreateTestEntity(i);
                    session.Store<TestEntity>(entity);
                    keys.Add(entity.Id);
                }    
            }
            _db.Dispose();

            _db = new JellyDatabase(_connectionString);
            _db.RegisterIdentityProperty<TestEntity, uint>(e => e.Id, true);
            foreach (var key in keys)
            {
                using (var session = _db.CreateSession())
                {
                    var retrieved = session.Load<uint, TestEntity>(key);
                    Assert.AreEqual(key, retrieved.Id);
                }
            }
        }
    }

    public class TestEntity
    {
        public TestEntity()
        {
            TextList = new List<string>();
        }
        public uint Id { get; set; }
        public int Number { get; set; }
        public string Text { get; set; }
        public List<String> TextList { get; set; }

        public static TestEntity CreateTestEntity(int num)
        {
            var result = new TestEntity()
            {
                Number = num,
                Text = Guid.NewGuid().ToString() + "-hi",
                TextList = new List<string>{
                    Guid.NewGuid().ToString()
                }
            };
            return result;
        }
    }
}
