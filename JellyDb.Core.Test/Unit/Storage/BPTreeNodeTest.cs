using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using JellyDb.Core.Engine.Fun;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JellyDb.Core.Test.Unit.Storage
{
    [TestClass]
    public class BPTreeNodeTest
    {
        [TestMethod]
        public void CreateSingleNode()
        {
            var node = new BPTreeNode<int, int>();
            node = node.Insert(1, 123);
            
            Assert.IsTrue(TestNode(node, 1, 123));
        }

        [TestMethod]
        public void CreateNodeAndInsertSequentially()
        {
            var node = new BPTreeNode<int, int>();
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
            Assert.IsTrue(TestNode(node, 3, 3));
            Assert.IsTrue(TestNode(node.Children[0], 1,1));
            Assert.IsTrue(TestNode(node.Children[0], 2, 2));
            Assert.IsTrue(TestNode(node.Children[1], 3, 3));
            Assert.IsTrue(TestNode(node.Children[1], 4, 4));
            Assert.IsTrue(TestNode(node.Children[1], 5, 5));

            node = node.Insert(6, 6);
            Assert.IsTrue(TestNode(node, 3, 3));
            Assert.IsTrue(TestNode(node.Children[0], 1, 1));
            Assert.IsTrue(TestNode(node.Children[0], 2, 2));
            Assert.IsTrue(TestNode(node.Children[1], 3, 3));
            Assert.IsTrue(TestNode(node.Children[1], 4, 4));
            Assert.IsTrue(TestNode(node.Children[1], 5, 5));
            Assert.IsTrue(TestNode(node.Children[1], 6, 6));

            node = node.Insert(7, 7);
            Assert.IsTrue(TestNode(node, 3, 3));
            Assert.IsTrue(TestNode(node, 5, 5));
            Assert.IsTrue(TestNode(node.Children[0], 1, 1));
            Assert.IsTrue(TestNode(node.Children[0], 2, 2));
            Assert.IsTrue(TestNode(node.Children[1], 3, 3));
            Assert.IsTrue(TestNode(node.Children[1], 4, 4));
            Assert.IsTrue(TestNode(node.Children[2], 5, 5));
            Assert.IsTrue(TestNode(node.Children[2], 6, 6));
            Assert.IsTrue(TestNode(node.Children[2], 7, 7));

            node = node.Insert(8, 8);
            Assert.IsTrue(TestNode(node, 3, 3));
            Assert.IsTrue(TestNode(node, 5, 5));
            Assert.IsTrue(TestNode(node.Children[0], 1, 1));
            Assert.IsTrue(TestNode(node.Children[0], 2, 2));
            Assert.IsTrue(TestNode(node.Children[1], 3, 3));
            Assert.IsTrue(TestNode(node.Children[1], 4, 4));
            Assert.IsTrue(TestNode(node.Children[2], 5, 5));
            Assert.IsTrue(TestNode(node.Children[2], 6, 6));
            Assert.IsTrue(TestNode(node.Children[2], 7, 7));
            Assert.IsTrue(TestNode(node.Children[2], 8, 8));

            node = node.Insert(9, 9);
            Assert.IsTrue(TestNode(node, 3, 3));
            Assert.IsTrue(TestNode(node, 5, 5));
            Assert.IsTrue(TestNode(node, 7, 7));
            Assert.IsTrue(TestNode(node.Children[0], 1, 1));
            Assert.IsTrue(TestNode(node.Children[0], 2, 2));
            Assert.IsTrue(TestNode(node.Children[1], 3, 3));
            Assert.IsTrue(TestNode(node.Children[1], 4, 4));
            Assert.IsTrue(TestNode(node.Children[2], 5, 5));
            Assert.IsTrue(TestNode(node.Children[2], 6, 6));
            Assert.IsTrue(TestNode(node.Children[3], 7, 7));
            Assert.IsTrue(TestNode(node.Children[3], 8, 8));
            Assert.IsTrue(TestNode(node.Children[3], 9, 9));

            node = node.Insert(10, 10);
            Assert.IsTrue(TestNode(node, 3, 3));
            Assert.IsTrue(TestNode(node, 5, 5));
            Assert.IsTrue(TestNode(node, 7, 7));
            Assert.IsTrue(TestNode(node.Children[0], 1, 1));
            Assert.IsTrue(TestNode(node.Children[0], 2, 2));
            Assert.IsTrue(TestNode(node.Children[1], 3, 3));
            Assert.IsTrue(TestNode(node.Children[1], 4, 4));
            Assert.IsTrue(TestNode(node.Children[2], 5, 5));
            Assert.IsTrue(TestNode(node.Children[2], 6, 6));
            Assert.IsTrue(TestNode(node.Children[3], 7, 7));
            Assert.IsTrue(TestNode(node.Children[3], 8, 8));
            Assert.IsTrue(TestNode(node.Children[3], 9, 9));
            Assert.IsTrue(TestNode(node.Children[3], 10, 10));

            node = node.Insert(11, 11);
            Assert.IsTrue(TestNode(node, 3, 3));
            Assert.IsTrue(TestNode(node, 5, 5));
            Assert.IsTrue(TestNode(node, 7, 7));
            Assert.IsTrue(TestNode(node, 9, 9));
            Assert.IsTrue(TestNode(node.Children[0], 1, 1));
            Assert.IsTrue(TestNode(node.Children[0], 2, 2));
            Assert.IsTrue(TestNode(node.Children[1], 3, 3));
            Assert.IsTrue(TestNode(node.Children[1], 4, 4));
            Assert.IsTrue(TestNode(node.Children[2], 5, 5));
            Assert.IsTrue(TestNode(node.Children[2], 6, 6));
            Assert.IsTrue(TestNode(node.Children[3], 7, 7));
            Assert.IsTrue(TestNode(node.Children[3], 8, 8));
            Assert.IsTrue(TestNode(node.Children[4], 9, 9));
            Assert.IsTrue(TestNode(node.Children[4], 10, 10));
            Assert.IsTrue(TestNode(node.Children[4], 11, 11));

            node = node.Insert(12, 12);
            Assert.IsTrue(TestNode(node, 3, 3));
            Assert.IsTrue(TestNode(node, 5, 5));
            Assert.IsTrue(TestNode(node, 7, 7));
            Assert.IsTrue(TestNode(node, 9, 9));
            Assert.IsTrue(TestNode(node.Children[0], 1, 1));
            Assert.IsTrue(TestNode(node.Children[0], 2, 2));
            Assert.IsTrue(TestNode(node.Children[1], 3, 3));
            Assert.IsTrue(TestNode(node.Children[1], 4, 4));
            Assert.IsTrue(TestNode(node.Children[2], 5, 5));
            Assert.IsTrue(TestNode(node.Children[2], 6, 6));
            Assert.IsTrue(TestNode(node.Children[3], 7, 7));
            Assert.IsTrue(TestNode(node.Children[3], 8, 8));
            Assert.IsTrue(TestNode(node.Children[4], 9, 9));
            Assert.IsTrue(TestNode(node.Children[4], 10, 10));
            Assert.IsTrue(TestNode(node.Children[4], 11, 11));
            Assert.IsTrue(TestNode(node.Children[4], 12, 12));

            node = node.Insert(13, 13);
            Assert.IsTrue(TestNode(node, 7, 7));
            Assert.IsTrue(TestNode(node.Children[0], 3, 3));
            Assert.IsTrue(TestNode(node.Children[0], 5, 5));
            Assert.IsTrue(TestNode(node.Children[1], 9, 9));
            Assert.IsTrue(TestNode(node.Children[1], 11, 11));
            Assert.IsTrue(TestNode(node.Children[0].Children[0], 1, 1));
            Assert.IsTrue(TestNode(node.Children[0].Children[0], 2, 2));
            Assert.IsTrue(TestNode(node.Children[0].Children[1], 3, 3));
            Assert.IsTrue(TestNode(node.Children[0].Children[1], 4, 4));
            Assert.IsTrue(TestNode(node.Children[0].Children[2], 5, 5));
            Assert.IsTrue(TestNode(node.Children[0].Children[2], 6, 6));
        }


        private bool TestNode(BPTreeNode<int,int> node, long key, long data)
        {
            if (node == null) return false;
            return node.Data.Any(n => n.Key == key && n.Value == data);
        }

    }
}
