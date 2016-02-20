using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;

using System.Threading;
using System.ComponentModel;

using FalconUDP;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Xna.Framework.Net
{
    public class MonoGameNetworkConfiguration
    {
        public static IPAddress Broadcast = IPAddress.None;
    }

    internal class FalconNetworkPeer
    {
        internal enum NetworkMessageType : byte
        {
            Data = 0,
            GamerJoined = 1,
            GamerLeft = 2,
            Introduction = 3,
            GamerProfile = 4,
            RequestGamerProfile = 5,
            GamerStateChange = 6,
            SessionStateChange = 7,
            DiscoveryRequest = 8,
            DiscoveryResponse = 9
        }

        FalconPeer peer;
        NetworkSession session;
        AvailableNetworkSession availableSession;

        public bool Online
        {
            get {
                return session.SessionType == NetworkSessionType.PlayerMatch;
            }
        }

        public bool IsReady
        {
            get { return peer != null && peer.IsStarted; }
        }

        public TimeSpan SimulatedLatency
        { 
            get { return peer.SimulateDelayTimeSpan; } 
            set { peer.SimulateDelayTimeSpan = value; }
        }
        public float SimulatedPacketLoss
        { 
            get { return (float)peer.SimulatePacketLossChance; } 
            set { peer.SimulatePacketLossChance = value; }
        }

        public FalconNetworkPeer(NetworkSession session, AvailableNetworkSession availableSession, bool allowDiscover = true)
        {
            this.session = session;
            this.availableSession = availableSession;
            this.peer = new FalconPeer(DefaultPort.Port, ProcessReceivedPacket, FalconPoolSizes.Default, (lvl, line) => {
                Console.WriteLine (line);
            }, LogLevel.Debug);
            if (allowDiscover)
            {
                this.peer.SetVisibility(true, null, allowDiscover, false, ApplicationIdentifier);
            }
            this.peer.PeerAdded += peer_PeerAdded;
            this.peer.PeerDiscovered += peer_PeerDiscovered;
            this.peer.PeerDropped += peer_PeerDropped;
            this.peer.DiscoveryRequest += peer_DiscoveryRequested;

            HookEvents();

            var result = this.peer.TryStart();
            if (!result.Success)
            {
                throw new NetworkException(result.NonSuccessMessage, result.Exception);
            }

            if (availableSession != null)
            {
                Exception ex = null;

                var packet = SendProfile(0);
                var e = new System.Threading.ManualResetEvent(false);
                this.peer.TryJoinPeerAsync(availableSession.IpEndpoint, callback: (i) => {
                    if (!i.Success)
                        ex = new NetworkException(i.NonSuccessMessage, i.Exception);
                    e.Set();
                }, userData: packet);
                bool done = false;
                Task.Run(() =>
                    {
                        while (!done)
                            Update();
                    });
                e.WaitOne();
                done = true;
                if (ex != null)
                    throw ex;
            }
        }

        void ProcessReceivedPacket(Packet packet)
        {
            NetworkMessageType mt = (NetworkMessageType)packet.ReadByte();
            CommandEvent command;
            switch (mt)
            {
                case NetworkMessageType.Data:
                    var dataLength = packet.ReadUInt32();
                    var data = packet.ReadBytes((int)dataLength);
                    command = new CommandEvent (
                        new CommandReceiveData (packet.PeerId,data)
                        );
                    session.commandQueue.Enqueue (command);     
                    break;
                case NetworkMessageType.GamerJoined:
                    command = new CommandEvent(new CommandGamerJoined(packet.PeerId));
                    session.commandQueue.Enqueue(command);
                    break;
                case NetworkMessageType.GamerLeft:
                    command = new CommandEvent(new CommandGamerLeft(packet.PeerId));
                    session.commandQueue.Enqueue(command);
                    break;
                case NetworkMessageType.Introduction:
                    break;
                case NetworkMessageType.GamerProfile:
                    packet.ReadUInt32();
                    var name = packet.ReadStringPrefixedWithSize(Encoding.ASCII);
                    packet.ReadUInt32();
                    packet.ReadUInt32();
                    var states = (GamerStates)packet.ReadUInt32();
                    command = new CommandEvent(new CommandGamerJoined(packet.PeerId) {
                        DisplayName = name, 
                        GamerTag = name,
                        remoteUniqueIdentifier = packet.PeerId,
                        State = states,
                        
                    });
                    session.commandQueue.Enqueue(command);
                    break;
                case NetworkMessageType.RequestGamerProfile:
                    break;
                case NetworkMessageType.GamerStateChange:
                    break;
                case NetworkMessageType.SessionStateChange:
                    command = new CommandEvent(new CommandSessionStateChange(NetworkSessionState.Playing, NetworkSessionState.Playing));
                    session.commandQueue.Enqueue(command);
                    break;
                case NetworkMessageType.DiscoveryRequest:
                    break;
                case NetworkMessageType.DiscoveryResponse:
                    break;
                default:
                    break;
            }
        }

        void peer_PeerDropped(int id)
        {
            
        }

        void peer_PeerDiscovered(IPEndPoint ipEndPoint, Packet packet)
        {
            //
            //var session = new AvailableNetworkSession();
            //session.IpEndpoint = ipEndPoint;
        }

        void peer_PeerAdded(int id, Packet userData)
        {
            if (userData == null)
                throw new ArgumentException("userData");

            userData.ReadByte();
            userData.ReadUInt32();
            var name = userData.ReadStringPrefixedWithSize(Encoding.ASCII);
            userData.ReadUInt32();
            userData.ReadUInt32();
            var states = (GamerStates)userData.ReadUInt32();

            var cmd = new CommandEvent(new CommandGamerJoined(id)
                {
                    DisplayName = name, 
                    GamerTag = name,
                    remoteUniqueIdentifier = id,
                });
            this.session.commandQueue.Enqueue(cmd);

            var packet = SendProfile(0);
            peer.EnqueueSendTo(id, SendOptions.Reliable, packet);
        }

        void peer_DiscoveryRequested(out Packet data)
        {
            data = this.peer.BorrowPacketFromPool();
            data.WriteInt32(session.AllGamers.Count);
            data.WriteStringPrefixSize(session.LocalGamers[0].Gamertag, Encoding.ASCII);
            data.WriteInt32((Int32)session.PrivateGamerSlots);
            data.WriteInt32((Int32)session.MaxGamers);
            data.WriteInt32((Int32)session.LocalGamers[0].State);
            for (int i = 0; i < 8; i++)
            {
                data.WriteBool(session.SessionProperties[i].HasValue);
                if (session.SessionProperties[i].HasValue)
                    data.WriteInt32(session.SessionProperties[i].Value);
                else
                    data.WriteInt32(0);
            }
        }

        private void HookEvents()
        {
            if (session != null)
            {
                session.GameEnded += HandleSessionStateChanged;
                session.SessionEnded += HandleSessionStateChanged;
                session.GameStarted += HandleSessionStateChanged;
            }
        }

        public void SendGamerStateChange(NetworkGamer gamer)
        {
            var om = peer.BorrowPacketFromPool();
            om.WriteByte((byte)NetworkMessageType.GamerStateChange);
            om.WriteInt32((int)gamer.State);

            peer.EnqueueSendToAll (SendOptions.Reliable, om);
        }

        void HandleSessionStateChanged(object sender, EventArgs e)
        {
            SendSessionStateChange();
            if (session.SessionState == NetworkSessionState.Ended)
                peer.Stop();
        }

        internal void SendSessionStateChange()
        {
            var packet = peer.BorrowPacketFromPool ();
            packet.WriteByte((byte) NetworkMessageType.SessionStateChange);
            packet.WriteInt32((int)session.SessionState);
            peer.EnqueueSendToAll(SendOptions.Reliable, packet);
        }

        internal void SendPeerIntroductions(NetworkGamer gamer)
        {
        }

        internal Packet SendProfile(int player)
        {
            var packet = peer.BorrowPacketFromPool();
            packet.WriteByte((byte)NetworkMessageType.GamerProfile);
            packet.WriteInt32(session.AllGamers.Count);
            packet.WriteStringPrefixSize(session.LocalGamers[0].Gamertag, Encoding.ASCII);
            packet.WriteInt32(session.PrivateGamerSlots);
            packet.WriteInt32(session.MaxGamers);
            packet.WriteInt32((int)session.LocalGamers[0].State);
            return packet;
        }

        internal void SendDiscoveryRequest()
        {
            var packet = peer.BorrowPacketFromPool();
            packet.WriteByte((byte)NetworkMessageType.DiscoveryRequest);
            peer.EnqueueSendTo(0, SendOptions.Reliable, packet);
        }

        public void SendData(
            byte[] data,
            SendDataOptions options)
        {
            this.SendData(data, options, null);
        }

        public void SendData(
            byte[] data,
            SendDataOptions options,
            NetworkGamer gamer)
        {
            SendOptions opts = SendOptions.None;
            switch (options)
            {
                case SendDataOptions.InOrder:
                    opts = SendOptions.InOrder;
                    break;
                case SendDataOptions.Reliable:
                    opts = SendOptions.Reliable;
                    break;
                case SendDataOptions.ReliableInOrder:
                    opts = SendOptions.ReliableInOrder;
                    break;
            }
            var packet = peer.BorrowPacketFromPool();
            packet.WriteByte((byte)NetworkMessageType.Data);
            packet.WriteUInt32((uint)data.Length);
            packet.WriteBytes(data);
            peer.EnqueueSendToAll(opts, packet);
        }

        public void ShutDown()
        {
            if (session != null)
            {
                session.GameEnded -= HandleSessionStateChanged;
                session.SessionEnded -= HandleSessionStateChanged;
                session.GameStarted -= HandleSessionStateChanged;
            }
            this.peer.DiscoveryRequest -= peer_DiscoveryRequested;
            this.peer.PeerAdded -= peer_PeerAdded;
            this.peer.PeerDiscovered -= peer_PeerDiscovered;
            this.peer.PeerDropped -= peer_PeerDropped;
            this.peer.Stop();
        }

        public void Update()
        {
            if (peer != null)
            {
                this.peer.Update();
                this.peer.SendEnquedPackets();
            }
        }

        public void Find(NetworkSessionType sessionType, IList<AvailableNetworkSession> sessions)
        {
            if (!peer.IsStarted)
                return;
            var e = new System.Threading.ManualResetEvent(false);
            bool done = false;
            Task.Factory.StartNew(() => {
                while (!done) {
                    Update ();
                }
            });
            peer.DiscoverFalconPeersAsync(TimeSpan.FromMilliseconds(5000), DefaultPort.DiscoveryPort, ApplicationIdentifier, (d, p) =>
                {
                    for (int x=0; x < d.Length; x++)
                    {
                        var session = new AvailableNetworkSession();
                        session.SessionProperties = new NetworkSessionProperties ();
                        session.IpEndpoint = d[x];

                        Packet packet = p[x];
                        if (packet != null)
                        {
                            session.CurrentGamerCount = packet.ReadInt32();//session.AllGamers.Count);
                            session.HostGamertag = packet.ReadStringPrefixedWithSize(Encoding.ASCII);// (session.LocalGamers[0].Gamertag);
                            session.OpenPrivateGamerSlots = packet.ReadInt32();//(session.PrivateGamerSlots);
                            session.OpenPublicGamerSlots = packet.ReadInt32();//(session.MaxGamers);
                            session.SessionType = sessionType;
                            packet.ReadInt32();//((int)session.LocalGamers[0].State);
                            for (int i = 0; i < 8; i++)
                            {
                                if (packet.ReadBool())
                                    session.SessionProperties[i] = packet.ReadInt32();
                                else
                                {
                                    session.SessionProperties[i] = null;
                                    packet.ReadInt32();
                                }
                            }
                        }
                        sessions.Add(session);
                    }
                    e.Set();
                });
            e.WaitOne();
            done = true;
        }

        internal static Guid? ApplicationIdentifier = default(Guid);

        static FalconNetworkPeer()
        {
#if !WINDOWS_PHONE
            // This code looks up the Guid for the host app , this is used to identify the
            // application on the network . We use the Guid as that is unique to that application.          
            var assembly = System.Reflection.Assembly.GetEntryAssembly();
            if (assembly != null)
            {
                object[] objects = assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false);
                if (objects.Length > 0)
                {
                    ApplicationIdentifier = Guid.Parse (((System.Runtime.InteropServices.GuidAttribute)objects[0]).Value);
                }           
            }
#endif
        }
    }

    public static class DefaultPort
    {
        public static int Port = 3075;
        public static int DiscoveryPort = 3075;
    }


}


#if WINDOWS_PHONE
namespace FindMyIP
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    public class MyIPAddress
    {
        Action<IPAddress> FoundCallback;
        UdpAnySourceMulticastClient MulticastSocket;
        const int PortNumber = 50000;       // pick a number, any number
        string MulticastMessage = "FIND-MY-IP-PLEASE" + new Random().Next().ToString();
 
        public void Find(Action<IPAddress> callback)
        {
            FoundCallback = callback;
 
            MulticastSocket = new UdpAnySourceMulticastClient(IPAddress.Parse("239.255.255.250"), PortNumber);
            MulticastSocket.BeginJoinGroup((result) =>
            {
                try
                {
                    MulticastSocket.EndJoinGroup(result);
                    GroupJoined(result);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("EndjoinGroup exception {0}", ex.Message);
                    // This can happen eg when wifi is off
                    FoundCallback(null);
                }
            },
                null);
        }
 
        void callback_send(IAsyncResult result)
        {
        }
 
        byte[] MulticastData;
        bool keepsearching;
 
        void GroupJoined(IAsyncResult result)
        {
            MulticastData = Encoding.UTF8.GetBytes(MulticastMessage);
            keepsearching = true;
            MulticastSocket.BeginSendToGroup(MulticastData, 0, MulticastData.Length, callback_send, null);
 
            while (keepsearching)
            {
                try
                {
                    byte[] buffer = new byte[MulticastData.Length];
                    MulticastSocket.BeginReceiveFromGroup(buffer, 0, buffer.Length, DoneReceiveFromGroup, buffer);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Stopped Group read due to " + ex.Message);
                    keepsearching = false;
                }
            }
        }
 
        void DoneReceiveFromGroup(IAsyncResult result)
        {
            IPEndPoint where;
            int responselength = MulticastSocket.EndReceiveFromGroup(result, out where);
            byte[] buffer = result.AsyncState as byte[];
            if (responselength == MulticastData.Length && buffer.SequenceEqual(MulticastData))
            {
                Debug.WriteLine("FOUND myself at " + where.Address.ToString());
                keepsearching = false;
                FoundCallback(where.Address);
            }
        }

        static ManualResetEvent _clientDone = new ManualResetEvent(false);

        public IPAddress Find()
        {
            var ip = IPAddress.None;
            _clientDone.Reset();
            Find((a) =>
            {
                ip = a;
                _clientDone.Set();
            });
            
            _clientDone.WaitOne(1000);
            return ip;
        }
    }
}
#endif
