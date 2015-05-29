using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class Index : BPTreeNode<long, DataPage>
    {
        private string _databaseName;
        private string _indexFileNameFormat = "{0}.index";

        private Index(string databaseName)
        {
            
        }
        
        public void SaveIndexToDisk()
        {
            
        }

        public static Index Load(string databaseName)
        {
            throw new NotImplementedException();
        }

        public static Index Create(string databaseName)
        {
            throw new NotImplementedException();
        }
    }
}
