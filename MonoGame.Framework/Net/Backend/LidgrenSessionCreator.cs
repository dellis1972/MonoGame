﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using Microsoft.Xna.Framework.GamerServices;

using Lidgren.Network;
using System.Net;

namespace Microsoft.Xna.Framework.Net.Backend.Lidgren
{
    internal class LidgrenSessionCreator : ISessionCreator
    {
        private const int DiscoveryTime = 1000;
        private const int FullyConnectedPollingTime = 50;
        private const int FullyConnectedTimeOut = 1000;

        private static bool WaitUntilFullyConnected(NetworkSession session)
        {
            int totalTime = 0;

            while (!session.IsFullyConnected)
            {
                if (totalTime > FullyConnectedTimeOut)
                {
                    return false;
                }

                session.SilentUpdate();

                Thread.Sleep(FullyConnectedPollingTime);
                totalTime += FullyConnectedPollingTime;
            }

            return true;
        }

        public NetworkSession Create(NetworkSessionType sessionType, IEnumerable<SignedInGamer> localGamers, int maxGamers, int privateGamerSlots, NetworkSessionProperties sessionProperties)
        {
            NetPeerConfiguration config = new NetPeerConfiguration(NetworkSessionSettings.AppId);
            config.Port = NetworkSessionSettings.Port;
            config.AcceptIncomingConnections = true;
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.NatIntroductionSuccess);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            NetPeer peer = new NetPeer(config);

            try
            {
                peer.Start();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Lidgren error: " + e.Message, e);
            }

            Debug.WriteLine("Peer started.");

            NetworkSession session = new NetworkSession(new LidgrenBackend(peer, null), maxGamers, privateGamerSlots, sessionType, sessionProperties, localGamers);

            if (!WaitUntilFullyConnected(session))
            {
                throw new NetworkException("Could not initialize session");
            }

            // Hack! TODO: Move into network session and perform regularly
            if (sessionType == NetworkSessionType.PlayerMatch || sessionType == NetworkSessionType.Ranked)
            {
                IPEndPoint masterServerEndPoint = NetUtility.Resolve(NetworkSessionSettings.MasterServerAddress, NetworkSessionSettings.MasterServerPort);
                IPAddress address;
                NetUtility.GetMyAddress(out address);

                OutgoingMessage msg = new OutgoingMessage();
                msg.Write((byte)MasterServerMessageType.RegisterHost);
                msg.Write(peer.UniqueIdentifier);
                msg.Write(new IPEndPoint(address, peer.Port));
                DiscoveryContents contents = new DiscoveryContents(session);
                contents.Pack(msg);

                NetOutgoingMessage request = peer.CreateMessage();
                request.Write(msg.Buffer);
                peer.SendUnconnectedMessage(request, masterServerEndPoint);

                Debug.WriteLine("Registering with master server (UID: " + peer.UniqueIdentifier + ", Address: " + address + ")");
            }

            return session;
        }

        private static void AddAvailableNetworkSession(long id, IPEndPoint endPoint, DiscoveryContents contents, IEnumerable<SignedInGamer> localGamers, NetworkSessionType searchType, NetworkSessionProperties searchProperties, IList<AvailableNetworkSession> availableSessions)
        {
            if (searchType == contents.sessionType && searchProperties.SearchMatch(contents.sessionProperties))
            {
                AvailableNetworkSession availableSession = new AvailableNetworkSession(endPoint, localGamers, contents.maxGamers, contents.privateGamerSlots, contents.sessionType, contents.currentGamerCount, contents.hostGamertag, contents.openPrivateGamerSlots, contents.openPublicGamerSlots, contents.sessionProperties);

                availableSession.Tag = id;

                availableSessions.Add(availableSession);
            }
        }

        public AvailableNetworkSessionCollection Find(NetworkSessionType sessionType, IEnumerable<SignedInGamer> localGamers, NetworkSessionProperties searchProperties)
        {
            IPEndPoint masterServerEndPoint = NetUtility.Resolve(NetworkSessionSettings.MasterServerAddress, NetworkSessionSettings.MasterServerPort);

            NetPeerConfiguration config = new NetPeerConfiguration(NetworkSessionSettings.AppId);
            config.Port = 0;
            config.AcceptIncomingConnections = false;
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.UnconnectedData);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            NetPeer discoverPeer = new NetPeer(config);

            try
            {
                discoverPeer.Start();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Lidgren error: " + e.Message, e);
            }

            Debug.WriteLine("Discovery peer started.");

            // Send discovery request
            if (sessionType == NetworkSessionType.SystemLink)
            {
                Debug.WriteLine("Sending local discovery request...");

                discoverPeer.DiscoverLocalPeers(NetworkSessionSettings.Port);
            }
            else if (sessionType == NetworkSessionType.PlayerMatch || sessionType == NetworkSessionType.Ranked)
            {
                Debug.WriteLine("Sending discovery request to master server...");

                NetOutgoingMessage request = discoverPeer.CreateMessage();
                request.Write((byte)MasterServerMessageType.RequestHosts);
                discoverPeer.SendUnconnectedMessage(request, masterServerEndPoint);
            }
            else
            {
                throw new InvalidOperationException();
            }

            // Wait for answers
            Thread.Sleep(DiscoveryTime);

            // Get list of answers
            List<AvailableNetworkSession> availableSessions = new List<AvailableNetworkSession>();

            NetIncomingMessage rawMsg;
            while ((rawMsg = discoverPeer.ReadMessage()) != null)
            {
                if (rawMsg.MessageType == NetIncomingMessageType.UnconnectedData)
                {
                    if (rawMsg.SenderEndPoint.Equals(masterServerEndPoint))
                    {
                        IIncomingMessage msg = new IncomingMessage(rawMsg);
                        long hostId = msg.ReadLong();
                        IPEndPoint hostEndPoint = msg.ReadIPEndPoint();
                        DiscoveryContents hostContents = new DiscoveryContents();
                        hostContents.Unpack(msg);

                        AddAvailableNetworkSession(hostId, hostEndPoint, hostContents, localGamers, sessionType, searchProperties, availableSessions);
                    }
                }
                else if (rawMsg.MessageType == NetIncomingMessageType.DiscoveryResponse)
                {
                    IIncomingMessage msg = new IncomingMessage(rawMsg);
                    DiscoveryContents hostContents = new DiscoveryContents();
                    hostContents.Unpack(msg);

                    AddAvailableNetworkSession(-1, rawMsg.SenderEndPoint, hostContents, localGamers, sessionType, searchProperties, availableSessions);
                }

                // Error checking
                switch (rawMsg.MessageType)
                {
                    case NetIncomingMessageType.VerboseDebugMessage:
                    case NetIncomingMessageType.DebugMessage:
                    case NetIncomingMessageType.WarningMessage:
                    case NetIncomingMessageType.ErrorMessage:
                        Debug.WriteLine("Lidgren: " + rawMsg.ReadString());
                        break;
                    default:
                        Debug.WriteLine("Unhandled type: " + rawMsg.MessageType);
                        break;
                }

                discoverPeer.Recycle(rawMsg);
            }

            discoverPeer.Shutdown("Discovery complete");

            Debug.WriteLine("Discovery peer shut down.");

            return new AvailableNetworkSessionCollection(availableSessions);
        }

        public NetworkSession Join(AvailableNetworkSession availableSession)
        {
            IPEndPoint masterServerEndPoint = NetUtility.Resolve(NetworkSessionSettings.MasterServerAddress, NetworkSessionSettings.MasterServerPort);

            NetPeerConfiguration config = new NetPeerConfiguration(NetworkSessionSettings.AppId);
            config.Port = 0;
            config.AcceptIncomingConnections = true;
            config.EnableMessageType(NetIncomingMessageType.VerboseDebugMessage);
            config.EnableMessageType(NetIncomingMessageType.NatIntroductionSuccess);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest); // if peer becomes host in the future
            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval); // if peer becomes host in the future
            NetPeer peer = new NetPeer(config);

            try
            {
                peer.Start();
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Lidgren error: " + e.Message, e);
            }

            Debug.WriteLine("Peer started.");

            AvailableNetworkSession aS = availableSession;

            if (aS.SessionType == NetworkSessionType.SystemLink)
            {
                peer.Connect(aS.RemoteEndPoint);
            }
            else if (aS.SessionType == NetworkSessionType.PlayerMatch || aS.SessionType == NetworkSessionType.Ranked)
            {
                // Note: Actual connect call is handled by backend once nat introduction is successful
                IPAddress address;
                NetUtility.GetMyAddress(out address);

                NetOutgoingMessage msg = peer.CreateMessage();
                msg.Write((byte)MasterServerMessageType.RequestIntroduction);
                msg.Write(new IPEndPoint(address, peer.Port));
                msg.Write((long)availableSession.Tag);
                peer.SendUnconnectedMessage(msg, masterServerEndPoint);
            }
            else
            {
                throw new InvalidOperationException();
            }

            NetworkSession session = new NetworkSession(new LidgrenBackend(peer, aS.RemoteEndPoint), aS.MaxGamers, aS.PrivateGamerSlots, aS.SessionType, aS.SessionProperties, aS.LocalGamers);

            if (!WaitUntilFullyConnected(session))
            {
                throw new NetworkSessionJoinException("Could not fully connect to session");
            }

            return session;
        }
    }
}