using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace Lidgren.Network
{
    /// <summary>
    /// Platform Specific implementation of the Socket Class    
    /// </summary>
    public class PlatformSocket
    {
#if !NETFX_CORE
        private Socket socket;
#else
        Windows.Networking.Sockets.DatagramSocket socket;
        bool broadcast = false;
#endif

        /// <summary>
        /// 
        /// </summary>
        public PlatformSocket()
        {
#if !NETFX_CORE
            this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
#else
            this.socket = new Windows.Networking.Sockets.DatagramSocket();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Broadcast
        {
            set
            {
#if !NETFX_CORE
                this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, value);
#else
                this.broadcast = value;
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Available
        {
            get
            {
#if !NETFX_CORE
                return this.socket.Available; 
#else
                return 0;
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReceiveBufferSize
        {
            get
            {
#if !NETFX_CORE
                return this.socket.ReceiveBufferSize; 
#else
                return 0;
#endif
            }
            set
            {
#if !NETFX_CORE
                this.socket.ReceiveBufferSize = value; 
#else
                throw new NotImplementedException();
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int SendBufferSize
        {
            get
            {
#if !NETFX_CORE
                return this.socket.SendBufferSize; 
#else
                throw new NotImplementedException();
#endif
            }
            set
            {
#if !NETFX_CORE
                this.socket.SendBufferSize = value; 
#else
                throw new NotImplementedException();
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Blocking
        {
            get
            {
#if !NETFX_CORE
                return this.socket.Blocking; 
#else
                throw new NotImplementedException();
#endif
            }
            set
            {
#if !NETFX_CORE
                this.socket.Blocking = value; 
#else
                throw new NotImplementedException();
#endif
            }
        }

        internal void Bind(System.Net.EndPoint ep)
        {
#if !NETFX_CORE
            this.socket.Bind(ep);
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public System.Net.EndPoint LocalEndPoint
        {
            get
            {
#if !NETFX_CORE
                return this.socket.LocalEndPoint; 
#else
                throw new NotImplementedException();
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsBound
        {
            get
            {
#if !NETFX_CORE
                return this.socket.IsBound; 
#else
                throw new NotImplementedException();
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="timeout"></param>
        public void Close(int timeout)
        {
#if !NETFX_CORE
            this.socket.Close(timeout);
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public bool DontFragment
        {
            get
            {
#if !NETFX_CORE                
                return this.socket.DontFragment; 
#else
                throw new NotImplementedException();
#endif
            }
            set
            {
#if !NETFX_CORE
                this.socket.DontFragment = value; 
#else
                throw new NotImplementedException();
#endif
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="microseconds"></param>
        /// <returns></returns>
        public bool Poll(int microseconds)
        {
#if !NETFX_CORE
            return this.socket.Poll(microseconds, SelectMode.SelectRead);
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="receiveBuffer"></param>
        /// <param name="offset"></param>
        /// <param name="numBytes"></param>
        /// <param name="senderRemote"></param>
        /// <returns></returns>
        public int ReceiveFrom(byte[] receiveBuffer, int offset, int numBytes, ref System.Net.EndPoint senderRemote)
        {
#if !NETFX_CORE
            return this.socket.ReceiveFrom(receiveBuffer, offset, numBytes, SocketFlags.None, ref senderRemote);
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="numBytes"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public int SendTo(byte[] data, int offset, int numBytes, System.Net.EndPoint target)
        {
#if !NETFX_CORE
            return this.socket.SendTo(data, offset, numBytes, SocketFlags.None, target);
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="socketShutdown"></param>
        public void Shutdown(SocketShutdown socketShutdown)
        {
#if !NETFX_CORE
            this.socket.Shutdown(socketShutdown);
#else
            throw new NotImplementedException();
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        public void Setup()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="res"></param>
        public void EndSendTo(IAsyncResult res)
        {

        }
    }
}