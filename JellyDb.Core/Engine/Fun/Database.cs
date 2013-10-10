using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JellyDb.Core.Storage;

namespace JellyDb.Core.Engine.Fun
{
    public class Database
    {
        private BPTreeNode<long, long> _pageIndex;
        private IDataStorage _dataStorage;

    }
}
