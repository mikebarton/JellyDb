using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Test.Performance
{
    [Serializable]
    public class BranchingFactorGroup
    {
        public BranchingFactorGroup()
        {
            SequentialResults = new List<BranchingFactorGroupResult>();
            UnSequentialResults = new List<BranchingFactorGroupResult>();
        }
        public int BranchingFactor { get; set; }
        public List<BranchingFactorGroupResult> SequentialResults { get; set; }
        public List<BranchingFactorGroupResult> UnSequentialResults { get; set; }
    }
}
