﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Sql.Dml
{
    public class ByteArrayTerm :ValueTerm<byte[]>
    {
        public override byte[] Value
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
