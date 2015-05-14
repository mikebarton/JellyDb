using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Engine.Fun;
using System.ComponentModel;
using System.Diagnostics;

namespace JellyDb.Visualisations.ViewModels
{
    public class BTreeViewModel : INotifyPropertyChanged
    {
        public BTreeViewModel()
        {
            BPTreeNode<int, int> node = new BPTreeNode<int, int>(3);
            TreeNode = node;
            Stopwatch stop = new Stopwatch();
            stop.Start();

            node = node.Insert(5, 5);
            node = node.Insert(1, 1);
            node = node.Insert(3, 3);
            node = node.Insert(2, 2);

            node = node.Insert(4, 4);
            node = node.Insert(22, 22);
            node = node.Insert(8, 8);
            node = node.Insert(14, 14);
            node = node.Insert(17, 17);
            node = node.Insert(20, 20);
            node = node.Insert(16, 16);
            node = node.Insert(18, 18);
            //for (int i = 1; i <= 99; i++)
            //{
            //    //var num = i;

            //    stop.Stop();
            //    var num = GenerateRandomNumber();
            //    stop.Start();

            //    node = node.Insert(num, num);
            //}
            stop.Stop();

            TreeNode = node;
        }

        private List<int> results = new List<int>();
        public int GenerateRandomNumber(Random random = null)
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
