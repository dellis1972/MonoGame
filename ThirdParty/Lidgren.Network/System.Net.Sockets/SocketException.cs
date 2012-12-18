using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Net.Sockets
{
    public class SocketException : Exception
    {
        public SocketError SocketErrorCode { get; set; }
    }
}
