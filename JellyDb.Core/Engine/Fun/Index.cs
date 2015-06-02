using JellyDb.Core.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class Index
    {
        private static string _fileName;
        private static string _indexFileNameFormat = "{0}.index";
        private BPTreeNode<long, DataItem> _indexTree;

        public Index()
        {
            _indexTree = new BPTreeNode<long, DataItem>(15);
        }

        public void Insert(long key, DataItem value)
        {
            _indexTree = _indexTree.Insert(key, value);
        }

        public DataItem Query(long key)
        {
            return _indexTree.Query(key);
        }
        
        public void SaveIndexToDisk()
        {
            using(var stream = File.Open(_fileName,FileMode.OpenOrCreate))
            using (var writer = new StreamWriter(stream))
            {
                var json = JsonConvert.SerializeObject(this);
                writer.Write(json);
            }
        }

        public static Index CreateOrLoad(string databaseName)
        {
            var folderPath = DbEngineConfigurationSection.ConfigSection.FolderPath;
            var fileName = Path.Combine(folderPath, string.Format(_indexFileNameFormat, databaseName));
            if (File.Exists(fileName))
            {
                using (var stream = File.Open(fileName, FileMode.OpenOrCreate))
                using (TextReader reader = new StreamReader(stream))
                {
                    var json = reader.ReadToEnd();
                    var index = JsonConvert.DeserializeObject<Index>(json);
                    index.FileName = fileName;
                    return index;
                }
            }
            return new Index() {FileName = fileName};
        }

        public BPTreeNode<long,DataItem> IndexData
        {
            get { return _indexTree; }
            set { _indexTree = value; }
        }

        private string FileName 
        {
            set { _fileName = value; }
        }
    }
}
