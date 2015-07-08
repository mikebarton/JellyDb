using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public interface IKeyGenerator
    {
        DataKey GenerateKey(object entity);
        void RegisterAutoGenIdentity(object autoGenerator);
    }
}
