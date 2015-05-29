using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Engine.Fun;
using System.ComponentModel;
using System.Diagnostics;
using JellyDb.Visualisations.Commands;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace JellyDb.Visualisations.ViewModels
{
    public class BTreeViewModel : INotifyPropertyChanged
    {
        private List<int> _history = new List<int>();
        public BTreeViewModel()
        {
            BPTreeNode<int, int> node = new BPTreeNode<int, int>(20);
            TreeNode = node;
            Stopwatch stop = new Stopwatch();
            stop.Start();

            for (int i = 1; i <= 999; i++)
            {
                //var num = i;

                //stop.Stop();
                var num = GenerateRandomNumber();
                //stop.Start();

                node = node.Insert(num, num);
            }
            //stop.Stop();

            TreeNode = node;
            
            InsertCommand = new DelegateCommand(o => !string.IsNullOrEmpty(TextValue), o =>
            {
                int val = 0;
                if(int.TryParse(TextValue, out val))
                {
                    TreeNode = TreeNode.Insert(val, val);
                    PropertyChanged(this, new PropertyChangedEventArgs("Root"));
                    _history.Add(val);
                }
                else
                {
                    var elems = TextValue.Split(new[]{","}, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var e in elems)
                    {
                        int v = 0;
                        if (int.TryParse(e, out v))
                        {
                            TreeNode = TreeNode.Insert(v, v);
                            PropertyChanged(this, new PropertyChangedEventArgs("Root"));
                            _history.Add(v);
                        }
                    }
                }
                TextValue = null;
                PropertyChanged(this, new PropertyChangedEventArgs("TextValue"));
                PropertyChanged(this, new PropertyChangedEventArgs("History"));
                PropertyChanged(this, new PropertyChangedEventArgs("HistoryString"));

            });
            ResetCommand = new DelegateCommand(o => true, o =>
            {
                TreeNode = new BPTreeNode<int, int>(100);
                PropertyChanged(this, new PropertyChangedEventArgs("Root"));
                _history = new List<int>();
                PropertyChanged(this, new PropertyChangedEventArgs("History"));
            });
            SaveCommand = new DelegateCommand(o => true, o =>
            {
                SaveFileDialog dialog = new SaveFileDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    using(var stream = File.Create(dialog.FileName))
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(BPTreeNode<int, int>));
                        ser.Serialize(stream, TreeNode);
                    }
                }
            });
            //TextValue = "50,100,20,150,40,10,30,18,2,19,15";
        }

        private List<int> results = new List<int>();
        private int GenerateRandomNumber(Random random = null)
        {
            if (random == null) random = new Random();
            var result = random.Next(1, Int32.MaxValue);
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

        public List<int> History
        {
            get
            {
                return _history;
            }
            set
            {
                _history = value;
            }
        }

        public string HistoryString
        {
            get { return string.Join(",", History); }
        }

        private string _textValue;
        public string TextValue
        {
            set 
            {
                _textValue = value;
                InsertCommand.NotifyCanExecuteChanged();
            }
            get { return _textValue; }
        }

        public List<BPTreeNode<int, int>> Root
        {
            get { return new List<BPTreeNode<int, int>>() { _node }; }
        }

        public DelegateCommand InsertCommand { get; set; }
        public DelegateCommand ResetCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
