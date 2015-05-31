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
        private string _databaseName;
        private string _fileName;
        private static string _indexFileNameFormat = "{0}.index";

        private Index(string databaseName, string fileName)
            : base(15)
        {
            _fileName = fileName;
            _databaseName = databaseName; 
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
                    var index = JsonConvert.DeserializeObject<Index>(reader.ReadToEnd());
                    return index;
                }
            }
            else
            {
                return new Index(databaseName, fileName);
            }
        }
    }
}
