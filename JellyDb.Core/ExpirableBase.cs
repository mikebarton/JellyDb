using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace JellyDb.Core
{
    internal delegate void ItemExpiredEventHandler(ExpirableBase expired);
    public abstract class ExpirableBase
    {
        private int ticksUntilExpiration;
        private int ticksElapsed;
        private bool inUse;
        internal event ItemExpiredEventHandler ItemExpired;

        public ExpirableBase(int secondsUntilExpiration)
        {
            inUse = false;
            ticksElapsed = 0;
            this.ticksUntilExpiration = secondsUntilExpiration;
        }

        internal abstract void Expire();

        internal void TickElapsed()
        {
            ticksElapsed++;
            if (ticksUntilExpiration == ticksElapsed && !inUse)
            {
                if (ItemExpired != null) ItemExpired(this);
                Expire();                
            }
        }

        public bool InUse
        {
            get { return inUse; }
            set { inUse = value; }
        }

        public void ExtendLifetime(int ticksToAdd)
        {
            ticksUntilExpiration += ticksToAdd;
        }
    }
}
