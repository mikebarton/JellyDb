using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Storage;

namespace JellyDb.Visualisations.ViewModels
{
    public class IntTreeNode : BPTreeNode<int,int>
    {
        public IntTreeNode(int num) : base(num)
        {

        }
    }
}
