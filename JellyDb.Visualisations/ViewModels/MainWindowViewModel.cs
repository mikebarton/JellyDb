using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Storage;
using System.ComponentModel;

namespace JellyDb.Visualisations.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            var node = new BPTreeNode();
            for (int i = 1; i < 17; i++)
            {
                node = node.Insert(i, i);
            }
            node = node.Insert(17, 17);
            TreeNode = node;
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
