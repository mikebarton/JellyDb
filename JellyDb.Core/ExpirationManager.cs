using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.MemoryMappedFiles;
using System.ComponentModel;
using System.Timers;
using System.Threading;
//using System.Threading;

namespace JellyDb.Core
{
    
    public class ExpirationManager : IDisposable
    {
        private static ExpirationManager instance;
        private BackgroundWorker workerThread;
        private List<ExpirableBase> expirables;
        private bool isShuttingDown = false;
        private static object syncObject;

        private ExpirationManager()
	    {
            syncObject = new object();
		    expirables = new List<ExpirableBase>();
            workerThread = new BackgroundWorker();
            workerThread.DoWork += new DoWorkEventHandler(workerThread_DoWork);
            workerThread.RunWorkerAsync();
    	}

        static ExpirationManager()
        {
            instance = new ExpirationManager();
        }

        public static void RegisterItemForExpiration(ExpirableBase expirable)
        {
            expirable.ItemExpired += new ItemExpiredEventHandler(ItemExpired);
            lock (syncObject)
            {
                instance.expirables.Add(expirable);
            }
        }

        
        void workerThread_DoWork(object sender, DoWorkEventArgs e)
        {            
            while (!isShuttingDown)
            {
                Thread.Sleep(5000);
                foreach (var item in expirables)
                {
                    item.TickElapsed();
                }
            }    
        }

        internal static void ItemExpired(ExpirableBase expired)
        {
            lock (syncObject)
            {
                instance.expirables.Remove(expired);
            }
        }

        public void Dispose()
        {
            isShuttingDown = true;
            foreach (var item in expirables)
            {
                item.Expire();                
            }            
        }

    }
}
