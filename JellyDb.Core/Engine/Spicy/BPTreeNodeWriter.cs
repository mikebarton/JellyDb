using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Spicy
{
    public class BPTreeNodeWriter
    {
        private static BPTreeNodeWriter _instance;
        private static object _instanceSyncLock = new object();

        public BPTreeNodeWriter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_instanceSyncLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new BPTreeNodeWriter();
                        }
                    }
                }
                return _instance;
            }
        }


    }
}
