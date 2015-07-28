using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Storage
{
    public class StorageWriteLock : IDisposable
    {
        private bool _isDisposed;

        public void Dispose(bool isDisposing)
        {
            if (!_isDisposed)
            {
                if (isDisposing)
                {

                }

                if (UnlockRequested != null) UnlockRequested(this, EventArgs.Empty);
            }
            _isDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~StorageWriteLock()
        {
            Dispose(false);
        }

        public event EventHandler UnlockRequested;
    }
}
