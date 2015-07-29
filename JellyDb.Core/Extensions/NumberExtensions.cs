using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JellyDb.Core.Extensions
{
    public static class NumberExtensions
    {
        public static int TruncateToInt32(this long input)
        {
            if (input > Int32.MaxValue) return Int32.MaxValue;
            else return (int)input;
        }

        public static int TruncateToInt32(this ulong input)
        {
            if (input > Int32.MaxValue) return Int32.MaxValue;
            else return (int)input;
        }

        public static int TruncateToInt32(this uint input)
        {
            if (input > Int32.MaxValue) return Int32.MaxValue;
            else return (int)input;
        }
    }
}
