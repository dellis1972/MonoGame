using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net
{
    public class IPEndPoint : EndPoint
    {
        private IPAddress adr;
        private int port;

        public IPEndPoint(IPAddress adr, int port)
        {
            // TODO: Complete member initialization
            this.adr = adr;
            this.port = port;
        }

        public IPAddress Address { get; set; }

        public int Port { get; set; }

        internal byte[] GetAddressBytes()
        {
            throw new NotImplementedException();
        }
    }
}
