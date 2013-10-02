using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Storage;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using JellyDb.Core.Test.Performance;
using System.IO;

namespace JellyDb.Visualisations.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            TestResults = new JellyDb.Core.Test.Performance.PerformanceTestResults();

            var node = new BPTreeNode(5);
            Stopwatch stop = new Stopwatch();
            stop.Start();
            for (int i = 1; i < 20; i++)
            {
                try
                {
                    var num = i;

                    //stop.Stop();
                    //var num = GenerateRandomNumber();
                    //stop.Start();
                    
                    node = node.Insert(num, num);
                }
                catch (Exception e)
                {

                }
            }
            stop.Stop();
            
            TreeNode = node;

            XmlSerializer ser = new XmlSerializer(typeof(PerformanceTestResults));
            using(FileStream stream = File.OpenRead(@"..\..\Resources\PerfTestResults.xml"))
            {
                var result = ser.Deserialize(stream) as PerformanceTestResults;
                TestResults = result;
            }
        }

        private List<int> results = new List<int>();
        public long GenerateRandomNumber(Random random = null)
        {
            if(random == null) random = new Random();
            var result = random.Next(1, 100);
            if (!results.Contains(result))
            {
                results.Add(result);
                return result;
            }
            return GenerateRandomNumber(random);
        }

        private BPTreeNode _node;

        public BPTreeNode TreeNode
        {
            get { return _node; }
            set
            {
                _node = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("TreeNode"));
            }
        }

        public Core.Test.Performance.PerformanceTestResults TestResults { get; set; }

        public List<BPTreeNode> Root 
        {
            get { return new List<BPTreeNode>() {_node}; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
