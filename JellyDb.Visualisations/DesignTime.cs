using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Visualisations;
using JellyDb.Visualisations.ViewModels;

namespace JellyDb.Visualisations
{
    public class DesignTime
    {
        

        public static MainWindowViewModel MainViewModel
        {
            get { return new MainWindowViewModel(); }
        }
    }
}
