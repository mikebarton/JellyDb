using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Storage;
using System.ComponentModel;
using System.Diagnostics;

namespace JellyDb.Visualisations.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
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
            //node = node.Insert(2, 2);
            //node = node.Insert(7, 7);
            //node = node.Insert(12, 12);
            //node = node.Insert(1, 1);
            //node = node.Insert(6, 6);
            //node = node.Insert(15, 15);
            //node = node.Insert(22, 22);
            //node = node.Insert(3, 3);
            TreeNode = node;
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

        public List<BPTreeNode> Root 
        {
            get { return new List<BPTreeNode>() {_node}; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
