using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using JellyDb.Core.Storage;

namespace JellyDb.Core.Test.Performance
{
    [Serializable]
    public class BranchingFactorGroupResult
    {
        public BranchingFactorGroupResult()
        {
            ReadResults = new List<long>();
        }
        [XmlIgnore]
        public Dictionary<int, int> UsedNumbers = new Dictionary<int, int>();

        [XmlIgnore]
        public BPTreeNode<int,int> NodeResult { get; set; }

        //public List<int> InsertedNumbers
        //{
        //    get { return UsedNumbers.Keys.ToList(); }
        //    set
        //    {
        //        UsedNumbers = new Dictionary<int, int>();
        //        if (value != null)
        //        {
        //            foreach (var num in value)
        //            {
        //                UsedNumbers[num] = num;
        //            }
        //        }
        //    }
        //}
        public long TestResult { get; set; }
        public List<long> ReadResults { get; set; }
    }
}
