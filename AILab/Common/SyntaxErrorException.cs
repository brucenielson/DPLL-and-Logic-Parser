using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AILab.Common
{
    public class SyntaxErrorException : Exception
    {
        public SyntaxErrorException() : base() { }

        public SyntaxErrorException(string message) : base(message) { }

        public SyntaxErrorException(string message, Exception innerException) : base(message, innerException) { }
    }

}
