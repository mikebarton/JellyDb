using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public class AutoGenIdentity
    {
        private uint _nextId;

        public AutoGenIdentity()
        {
            
        }

        public uint GetNextId()
        {
            var result = _nextId;
            _nextId++;
            NextIdentityRetrieved(this, EventArgs.Empty);
            return result;
        }

        public uint CurrentUsedId { get { return _nextId--; } set { _nextId = value++; } }

        public event EventHandler NextIdentityRetrieved; 

        public string EntityTypeName { get; set; }
    }
}
