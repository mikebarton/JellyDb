using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.Storage;

namespace JellyDb.Core.Test.Unit.Storage
{
    [TestClass]
    public class BPTreeNodeTest
    {
        [TestMethod]
        public void CreateSingleNode()
        {
            var node = new BPTreeNode(1,123);
            
            Assert.IsTrue(TestNode(node, 1, 123));
            
        }

        [TestMethod]
        public void CreateNodeAndInsertSequentially()
        {
            var node = new BPTreeNode();
            node = node.Insert(1, 1);
            Assert.IsTrue(TestNode(node, 1, 1));

            node = node.Insert(2, 2);
            Assert.IsTrue(TestNode(node, 1, 1));
            Assert.IsTrue(TestNode(node, 2, 2));

            node = node.Insert(3, 3);
            Assert.IsTrue(TestNode(node, 1, 1));
            Assert.IsTrue(TestNode(node, 2, 2));
            Assert.IsTrue(TestNode(node, 3, 3));

            node = node.Insert(4, 4);
            Assert.IsTrue(TestNode(node, 1, 1));
            Assert.IsTrue(TestNode(node, 2, 2));
            Assert.IsTrue(TestNode(node, 3, 3));
            Assert.IsTrue(TestNode(node, 4, 4));

            node = node.Insert(5, 5);
            Assert.IsTrue(TestNode(node, 1, 1));
            Assert.IsTrue(TestNode(node, 2, 2));
            Assert.IsTrue(TestNode(node, 3, 3));
            Assert.IsTrue(TestNode(node, 4, 4));
        }


        private bool TestNode(BPTreeNode node, long key, long data)
        {
            if (node == null) return false;
            return node.Data.Any(n => n.Key == key && n.Value == data);
        }

    }
}
