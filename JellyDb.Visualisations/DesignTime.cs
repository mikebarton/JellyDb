using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Visualisations.Controls;

namespace JellyDb.Visualisations
{
    public class DesignTime
    {
        public static MyTreeViewItem Node
        {
            get
            {
                return new MyTreeViewItem()
                    {
                        Width = 10,
                        Height = 10,
                        Key = 10
                    };
            }
        }

        public static MainWindow MainViewModel
        {
            get { return new MainWindow(); }
        }
    }
}
