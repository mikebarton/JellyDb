using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using JellyDb.Visualisations.ViewModels;

namespace JellyDb.Visualisations.Controls
{
    /// <summary>
    /// Interaction logic for BTree.xaml
    /// </summary>
    public partial class BTree : UserControl
    {
        public BTree()
        {
            InitializeComponent();
            DataContext = new BTreeViewModel();
        }
    }
}
