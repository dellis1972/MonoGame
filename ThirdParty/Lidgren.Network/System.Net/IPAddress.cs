using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Net
{
    public class IPAddress
    {
        private byte[] addressBytes;

        public IPAddress(byte[] addressBytes)
        {
            // TODO: Complete member initialization
            this.addressBytes = addressBytes;
        }
        internal static bool TryParse(string ipOrHost, out IPAddress ipAddress)
        {
            throw new NotImplementedException();
        }

        public AddressFamily AddressFamily { get; set; }

        public static IPAddress Broadcast { get; set; }

        public static IPAddress Any { get; set; }

        internal byte[] GetAddressBytes()
        {
            throw new NotImplementedException();
        }

        internal static IPAddress Parse(string IP)
        {
            throw new NotImplementedException();
        }
    }
}
