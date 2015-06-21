using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace JellyDb.Core.Engine.Fun
{
    public class StringComparer : ITypeComparer<string>
    {
        private const string _allowedCharsRegex = @"^[a-zA-Z\-.]{0,32}$";

        public string MinKey
        {
            get { return string.Empty; }
        }

        public string MaxKey
        {
            get { throw new NotImplementedException(); }
        }

        public int Compare(string one, string two)
        {
            if (one == null || two == null) throw new ArgumentNullException("one/two");
            return String.Compare(one, two);
        }

        public string Decrement(string input)
        {
            BitConverter.(BitConverter.GetBytes(Int32.MaxValue), 0);
        }

        public string Increment(string input)
        {
            throw new NotImplementedException();
        }

        private bool IncrementChar(char input, out char c)
        {
            input++;
            if (Regex.IsMatch(new string(new[] { input }), _allowedCharsRegex))
            {

            }
        }
    }
}
