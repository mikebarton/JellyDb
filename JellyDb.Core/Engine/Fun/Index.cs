using JellyDb.Core.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class Index : BPTreeNode<long, DataItem>
    {
        private static string _fileName;
        private static string _indexFileNameFormat = "{0}.index";

        public Index()
            : base(15)
        {}
        
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
            if (File.Exists(_fileName))
            {
                using (var stream = File.Open(_fileName, FileMode.OpenOrCreate))
                using (TextReader reader = new StreamReader(stream))
                {
                    var index = JsonConvert.DeserializeObject<Index>(reader.ReadToEnd());
                    index.FileName = fileName;
                    return index;
                }
            }
            return new Index() {FileName = fileName};
        }

        private string FileName 
        {
            set { _fileName = value; }
        }
    }
}
