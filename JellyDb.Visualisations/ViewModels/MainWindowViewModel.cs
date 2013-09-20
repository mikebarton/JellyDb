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
            node.Insert(1, 1);
            node.Insert(2, 2);
            node.Insert(3, 3);
            node.Insert(4, 4);
            node.Insert(5, 5);
            node.Insert(6, 6);
            _node = node;
        }

        private BPTreeNode _node;
        public BPTreeNode Node
        {
            get { return _node; }
            set 
            {
                _node = value;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Node"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
