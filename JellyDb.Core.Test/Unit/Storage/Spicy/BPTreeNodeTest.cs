using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using JellyDb.Core.Engine.Spicy;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JellyDb.Core.Test.Unit.Storage.Spicy
{
    [TestClass]
    public class BPTreeNodeTest
    {
        [TestMethod]
        public void CreateSingleNode()
        {
            var node = new BPTreeNode<int, int>(10);
            node = node.Insert(1, 123);
            
            Assert.IsTrue(TestNode(node, 1, 123));
        }

        [TestMethod]
        public void CreateNodeAndInsertSequentially()
        {
            var node = new BPTreeNode<int, int>(5);

            // insert arbitrary calc based on i to spice things up and know that it isn't just returning the key
            Func<int,int> calcData = num => ((int)(((num + 3) * 5) / 4));

            for (int i = 0; i < 1000; i++)
            {
                
                node = node.Insert(i, calcData(i)); 
                for (int j = 0; j <= i; j++)
                {
                    var retrieved = node.Query(j);
                    Assert.AreEqual(retrieved, calcData(j));
                }
            }              
        }

        [TestMethod]
        public void CreateTreeAndOverWriteSomeAndReadAgain()
        {
            var node = new BPTreeNode<int, TestObject>(15);
            for (int i = 0; i < 50; i++)
            {
                node = node.Insert(i, new TestObject() {Num = i.ToString()});
            }
            var result = node.Query(6);
            node = node.Insert(6, new TestObject() { Num = 6.ToString() + "a" });
            for (int i = 0; i < 10; i++)
            {
                node = node.Insert(i, new TestObject() { Num = i.ToString() + "b" });
            }
            for (int i = 0; i < 50; i++)
            {
                var retrieved = node.Query(i);
            }
        }

        public class TestObject
        {
            public string Num { get; set; }
        }


        private bool TestNode(BPTreeNode<int,int> node, long key, long data)
        {
            if (node == null) return false;
            return node.Data.Any(n => n.Key == key && n.Value == data);
        }

    }
}
