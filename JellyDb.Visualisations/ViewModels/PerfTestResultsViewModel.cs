using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Test.Performance;
using System.Xml.Serialization;
using System.IO;

namespace JellyDb.Visualisations.ViewModels
{
    public class PerfTestResultsViewModel
    {
        public PerfTestResultsViewModel()
        {
            XmlSerializer ser = new XmlSerializer(typeof(PerformanceTestResults));
            using (FileStream stream = File.OpenRead(@"..\..\Resources\PerfTestResults.xml"))
            {
                var result = ser.Deserialize(stream) as PerformanceTestResults;
                SequentialResults = new List<ChartArg>();
                foreach (var item in result.BranchFactorGroupResults)
                {
                    foreach (var inner in item.SequentialResults)
                    {
                        SequentialResults.Add(new ChartArg() { BranchFactor = item.BranchingFactor, TestResult = inner.TestResult });
                    }
                }

                UnSequentialResults = new List<ChartArg>();
                foreach (var item in result.BranchFactorGroupResults)
                {
                    foreach (var inner in item.UnSequentialResults)
                    {
                        UnSequentialResults.Add(new ChartArg() { BranchFactor = item.BranchingFactor, TestResult = inner.TestResult });
                    }
                }

                SequentialReadResults = new List<ChartArg>();
                foreach (var item in result.BranchFactorGroupResults)
                {
                    foreach (var inner in item.SequentialResults)
                    {
                        foreach (var read in inner.ReadResults)
                        {
                            SequentialReadResults.Add(new ChartArg() { BranchFactor = item.BranchingFactor, TestResult = read });
                        }
                    }
                }

                UnSequentialReadResults = new List<ChartArg>();
                foreach (var item in result.BranchFactorGroupResults)
                {
                    foreach (var inner in item.UnSequentialResults)
                    {
                        foreach (var read in inner.ReadResults)
                        {
                            UnSequentialReadResults.Add(new ChartArg() { BranchFactor = item.BranchingFactor, TestResult = read });
                        }
                    }
                }
            }
        }

        public List<ChartArg> SequentialResults { get; set; }
        public List<ChartArg> UnSequentialResults { get; set; }
        public List<ChartArg> SequentialReadResults { get; set; }
        public List<ChartArg> UnSequentialReadResults { get; set; }
    }

    public class ChartArg
    {
        public int BranchFactor { get; set; }
        public long TestResult { get; set; }
    }
}
