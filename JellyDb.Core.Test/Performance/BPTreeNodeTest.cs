using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using JellyDb.Core.Test.Performance;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JellyDb.Core.Storage;
using System.Diagnostics;
using System.Xml.Serialization;
using System.IO;

namespace JellyDb.Core.Test.Performance
{
    [TestClass]
    public class BPTreeNodeTest
    {
        [TestMethod]
        public void TimingTest()
        {
            List<int> branchingFactor = new List<int>() { 5, 10, 30, 50, 100, 150, 200, 300,500 };

            PerformanceTester tester = new PerformanceTester();
            tester.BranchFactors = branchingFactor;
            tester.InsertionsToPerform = 500000;

            var result = tester.Execute();

            if (File.Exists(@"C:\temp\nodeOutput.txt")) File.Delete(@"C:\temp\nodeOutput.txt");
           
            using(FileStream stream = File.Create(@"C:\temp\nodeOutput.txt"))
            {
                XmlSerializer ser = new XmlSerializer(typeof(PerformanceTestResults));
                ser.Serialize(stream, result);
            }
        }

        [TestMethod]
        public void IndexSequentialTest()
        {
            BPTreeNode<int, int> node = new BPTreeNode<int, int>(10);
            for (int i = 1; i <= 1000000; i++)
            {
                node = node.Insert(i, i);
            }

            if (File.Exists(@"C:\temp\node.xml")) File.Delete(@"C:\temp\node.xml");

            using (FileStream stream = File.Create(@"C:\temp\node.xml"))
            {
                XmlSerializer ser = new XmlSerializer(typeof(BPTreeNode<int, int>));
                ser.Serialize(stream, node);
            }
        }

        [TestMethod]
        public void IndexNonSequentialTest()
        {
            BPTreeNode<int, int> node = new BPTreeNode<int, int>(10);
            for (int i = 1; i <= 1000000; i++)
            {
                var num = GenerateRandomNumber();
                node = node.Insert(num, num);
            }

            if (File.Exists(@"C:\temp\node.xml")) File.Delete(@"C:\temp\node.xml");

            using (FileStream stream = File.Create(@"C:\temp\node.xml"))
            {
                XmlSerializer ser = new XmlSerializer(typeof(BPTreeNode<int, int>));
                ser.Serialize(stream, node);
            }
        }

        private List<int> results = new List<int>();
        public int GenerateRandomNumber(Random random = null)
        {
            if (random == null) random = new Random();
            var result = random.Next(1, 999999999);
            if (!results.Contains(result))
            {
                results.Add(result);
                return result;
            }
            return GenerateRandomNumber(random);
        }
    }

    public class PerformanceTester
    {
        public List<int> BranchFactors { get; set; }
        public int InsertionsToPerform { get; set; }

        private int GetRandomNumber(Dictionary<int,int> numberTracker, Random rnd = null)
        {
            return GetRandomNumber(1, 999999999, numberTracker, rnd);
        }

        private int GetRandomNumber(int min, int max, Dictionary<int, int> numberTracker, Random rnd = null)
        {
            if (rnd == null) rnd = new Random();
            var num = rnd.Next(min, max);
            if (numberTracker.ContainsKey(num)) return GetRandomNumber(numberTracker, rnd);
            else
            {
                numberTracker[num] = num;
                return num;
            }
        }

        public BranchingFactorGroupResult TestSequential(int branchFactor, int numInsertions)
        {
            var result = new BranchingFactorGroupResult();
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var node = new BPTreeNode<int, int>();
            node.BranchingFactor = branchFactor;

            for (int i = 1; i < numInsertions+1; i++)
            {
                node = node.Insert(i, i);
            }

            timer.Stop();
            result.TestResult = timer.ElapsedTicks;
            //result.NodeResult = node;

            for (int i = 0; i < 5; i++)
            {
                var randomNumber = GetRandomNumber(1, numInsertions, new Dictionary<int, int>());
                timer.Restart();
                var queryResult = node.Query(randomNumber);
                timer.Stop();
                result.ReadResults.Add(timer.ElapsedTicks);
            }
            return result;
        }

        public BranchingFactorGroupResult TestUnSequential(int branchFactor, int numInsertions)
        {
            var result = new BranchingFactorGroupResult();
            Stopwatch timer = new Stopwatch();
            timer.Start();
            var node = new BPTreeNode<int, int>();
            node.BranchingFactor = branchFactor;

            for (int i = 1; i < numInsertions + 1; i++)
            {
                timer.Stop();
                var num = GetRandomNumber(result.UsedNumbers);
                timer.Start();
                node = node.Insert(num, num);
            }

            timer.Stop();
            result.TestResult = timer.ElapsedTicks;
            //result.NodeResult = node;

            for (int i = 0; i < 5; i++)
            {
                var randomIndex = GetRandomNumber(0, result.UsedNumbers.Keys.Count-1, new Dictionary<int, int>());
                var randomNumber = result.UsedNumbers.Keys.ElementAt((int)randomIndex);
                timer.Restart();
                var queryResult = node.Query(randomNumber);
                timer.Stop();
                result.ReadResults.Add(timer.ElapsedTicks);
            }
            return result;
        }

        public PerformanceTestResults Execute()
        {
            PerformanceTestResults perfResults = new PerformanceTestResults();
            perfResults.NumberOfInsertionsPerTest = InsertionsToPerform;
            foreach (var branchFactor in BranchFactors)
            {
                BranchingFactorGroup groupResult = new BranchingFactorGroup();
                groupResult.BranchingFactor = branchFactor;

                for (int i = 0; i < 4; i++)
                {
                    var result = TestSequential(branchFactor, InsertionsToPerform);
                    groupResult.SequentialResults.Add(result);

                    var unresult = TestUnSequential(branchFactor, InsertionsToPerform);
                    groupResult.UnSequentialResults.Add(unresult);
                }
                perfResults.BranchFactorGroupResults.Add(groupResult);
            }
            return perfResults;
        }
    }

    

    

    
}
