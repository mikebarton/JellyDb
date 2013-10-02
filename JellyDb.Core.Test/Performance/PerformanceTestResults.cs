using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Test.Performance
{
    [Serializable]
    public class PerformanceTestResults
    {
        public PerformanceTestResults()
        {
            BranchFactorGroupResults = new List<BranchingFactorGroup>();
        }
        public int NumberOfInsertionsPerTest { get; set; }
        public List<BranchingFactorGroup> BranchFactorGroupResults { get; set; }
    }
}
