using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEngine2.Common.Net
{
    public class HttpHeaderException : Exception
    {
        public HttpHeaderException() : base()
        {
        }

        public HttpHeaderException(string message) : base(message)
        {
        }
    }
}