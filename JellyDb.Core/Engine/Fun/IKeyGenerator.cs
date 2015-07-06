﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Engine.Fun
{
    public interface IKeyGenerator
    {
        Type GetEntityType();
        DataKey GenerateKey(object entity);
    }
}
