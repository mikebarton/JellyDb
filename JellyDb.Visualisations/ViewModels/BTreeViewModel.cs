using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Storage;
using System.ComponentModel;
using System.Diagnostics;

namespace JellyDb.Visualisations.ViewModels
{
    public class BTreeViewModel : INotifyPropertyChanged
    {
        public BTreeViewModel()
        {
            BPTreeNode<int, int> node = new IntTreeNode(5);
            Stopwatch stop = new Stopwatch();
            stop.Start();
            for (int i = 1; i < 20; i++)
            {
                var num = i;

                //stop.Stop();
                //var num = GenerateRandomNumber();
                //stop.Start();

                node = node.Insert(num, num);
            }
            stop.Stop();

            TreeNode = node;
        }

        private List<int> results = new List<int>();
        public long GenerateRandomNumber(Random random = null)
        {
            if (random == null) random = new Random();
            var result = random.Next(1, 100);
            if (!results.Contains(result))
            {
                results.Add(result);
                return result;
            }
            return GenerateRandomNumber(random);
        }

        private BPTreeNode<int, int> _node;

        public BPTreeNode<int, int> TreeNode
        {
            get { return _node; }
            set
            {
                _node = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("TreeNode"));
            }
        }

        public List<BPTreeNode<int, int>> Root
        {
            get { return new List<BPTreeNode<int, int>>() { _node }; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
