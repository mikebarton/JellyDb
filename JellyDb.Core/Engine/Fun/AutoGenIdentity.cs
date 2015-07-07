using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class AutoGenIdentity
    {
        public AutoGenIdentity()
        {
            
        }

        public uint GetNextId()
        {
            CurrentUsedId++; ;
            var result = CurrentUsedId;
            NextIdentityRetrieved(this, EventArgs.Empty);
            return result;
        }

        public uint CurrentUsedId { get; set; }

        public event EventHandler NextIdentityRetrieved; 

        public string EntityTypeName { get; set; }
    }
}
