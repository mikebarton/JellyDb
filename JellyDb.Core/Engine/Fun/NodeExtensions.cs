using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JellyDb.Core.Engine.Fun
{
    public static class NodeExtensions
    {
        public static void Serialize<TKey, TData>(this BPTreeNode<long, IndexResult> node, BinaryWriter writer)
        {
            writer.Write(node.BranchingFactor);
            writer.Write(node.MinKey);
            writer.Write(node.MaxKey);
            foreach (var item in node.Data)
            {
                writer.Write(item.Key);
                writer.Write(item.Value.Offset);
                writer.Write(item.Value.Size);
            }

            foreach (BPTreeNode<long, IndexResult> item in node.Children)
            {
                item.Serialize<long, IndexResult>(writer);
            }
        }

    }
}
