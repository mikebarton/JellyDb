using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
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
        }


        [TestMethod]
        public void CreateDatabaseClient()
        {
            JellyDatabase db = new JellyDatabase(@"c:\temp\jelly\jelly.db");
            using(var session = db.CreateSession())
            {
                db.RegisterIdentityProperty<TestEntity, uint>(entity => entity.Id, true);
                session.Store(new TestEntity()
                    {
                        Number = 4,
                        Text = "hello",
                        TextList = new List<string>()
                            {
                                "one",
                                "two"
                            }
                    });
            }
        }
    }
}
