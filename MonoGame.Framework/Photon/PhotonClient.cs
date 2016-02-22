using System;
using ExitGames.Client.Photon.LoadBalancing;
using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.GamerServices;

namespace Microsoft.Xna.Framework.Net
{
    public class PhotonClient : LoadBalancingClient
    {
        internal int intervalDispatch = 50;                 // interval between DispatchIncomingCommands() calls
        internal int lastDispatch = Environment.TickCount;
        internal int intervalSend = 50;                     // interval between SendOutgoingCommands() calls
        internal int lastSend = Environment.TickCount;
        TypedLobby lobby = new TypedLobby ("Master", LobbyType.Default);
        NetworkSession session;

        internal class MonoGamePlayer : Player {
            internal MonoGamePlayer(string name, int id, bool isLocal) : base (name, id, isLocal)
            {
            }

            internal Gamer Gamer { get; set; }
        }

        internal class MonoGameRoom : Room {
            internal MonoGameRoom(string roomName, RoomOptions opt) : base (roomName, opt)
            {
            }
        }

        public PhotonClient(NetworkSession session = null) : base ()
        {
            MasterServerAddress = "app-eu.exitgamescloud.com:5055";
            AppId = "b20a3471-a51a-4a1c-8104-f882527fd5d6";
            AppVersion = "1.0";
            AutoJoinLobby = false;
            this.session = session;
        }

        protected internal override Player CreatePlayer(string actorName, int actorNumber, bool isLocal, Hashtable actorProperties)
        {
            if (actorNumber == -1)
                return new MonoGamePlayer(actorName, actorNumber, isLocal);
            Gamer gamer;
            if (isLocal || string.IsNullOrEmpty (actorName))
            {
                // grab the signed in gamer

                gamer = new LocalNetworkGamer(session, 0, GamerStates.Host | GamerStates.Local)
                {
                    Gamertag = SignedInGamer.SignedInGamers[0].Gamertag,
                    DisplayName = SignedInGamer.SignedInGamers[0].DisplayName,
                    SignedInGamer = SignedInGamer.SignedInGamers[0]
                };
                session.AllGamers.AddGamer((NetworkGamer)gamer);
                session.LocalGamers.AddGamer((LocalNetworkGamer)gamer);
            }
            else
            {
                gamer = new NetworkGamer(session, 0, GamerStates.Guest)
                {
                        DisplayName = actorName,
                        Gamertag = actorName,
                        RemoteUniqueIdentifier = actorNumber,
                };
                session.AllGamers.AddGamer((NetworkGamer)gamer);
                session.RemoteGamers.AddGamer((NetworkGamer)gamer);
            }
            return new MonoGamePlayer(gamer.Gamertag, actorNumber, isLocal) {
                Gamer = gamer,
            };
        }

        /// <summary>
        /// Override of the factory method used by the LoadBalancing framework (which we extend here) to create a Room instance.
        /// </summary>
        /// <remarks>
        /// While CreateParticleDemoRoom will make the server create the room, this method creates a local object to represent that room.
        /// 
        /// This method is used by a LoadBalancingClient automatically whenever this client joins or creates a room.
        /// We override it to produce a ParticleRoom which has more features like Map and GridSize.
        /// </remarks>
        protected internal override Room CreateRoom(string roomName, RoomOptions opt)
        {
            return new MonoGameRoom (roomName, opt);
        }

        private void OnStateChanged(ClientState clientState)
        {
            switch (clientState)
            {
                case ClientState.ConnectedToNameServer:
                    if (string.IsNullOrEmpty(this.CloudRegion))
                    {
                        this.OpGetRegions();
                    }
                    break;
                case ClientState.ConnectedToGameserver:
                    break;
                case ClientState.ConnectedToMaster:
                    // authentication concludes connecting to the master server (it sends the appId and identifies your game)
                    // when that's done, this demo asks the Master for any game. the result is handled below
                    //                    this.OpJoinRandomRoom(null, 0);
                    //this.CreateParticleDemoRoom(DemoConstants.MapType.Forest, 16);                    
                    break;

            }
        }

        public void Update()
        {
            if (Environment.TickCount - this.lastDispatch > this.intervalDispatch) {
                this.lastDispatch = Environment.TickCount;
                this.loadBalancingPeer.DispatchIncomingCommands ();
            }

            if (Environment.TickCount - this.lastSend > this.intervalSend) {
                this.lastSend = Environment.TickCount;
                this.loadBalancingPeer.SendOutgoingCommands (); // will send pending, outgoing commands
            }
        }

        public override void OnEvent(EventData photonEvent)
        {
            // most events have a sender / origin (but not all) - let's find the player sending this
            int actorNr = 0;
            Player origin = null;
            if (photonEvent.Parameters.ContainsKey(ParameterCode.ActorNr))
            {
                actorNr = (int)photonEvent[ParameterCode.ActorNr];  // actorNr (a.k.a. playerNumber / ID) of sending player
            }

            if (actorNr > 0)
            {
               // this.LocalRoom.Players.TryGetValue(actorNr, out origin);
            }

            base.OnEvent(photonEvent);  // important to call, to keep state up to date

            if (actorNr > 0 && origin == null)
            {
                //this.LocalRoom.Players.TryGetValue(actorNr, out origin);
            }
            /*
            // the list of players will only store Player references (not the derived class). simply cast:
            ParticlePlayer originatingPlayer = (ParticlePlayer)origin;

            // this demo logic doesn't handle any events from the server (that is done in the base class) so we could return here
            if (actorNr != 0 && originatingPlayer == null)
            {
                this.DebugReturn(DebugLevel.WARNING, photonEvent.Code + " ev. We didn't find a originating player for actorId: " + actorNr);
                return;
            }
            */
            // this demo defined 2 events: Position and Color. additionally, a event is triggered when players join or leave
            switch ((NetworkMessageType)photonEvent.Code)
            {
                case NetworkMessageType.GamerJoined:
                    break;
                case NetworkMessageType.GamerLeft:
                    break;
                case NetworkMessageType.GamerProfile:
                    break;
                case NetworkMessageType.GamerStateChange:
                    break;
                case NetworkMessageType.RequestGamerProfile:
                    break;
                case NetworkMessageType.SessionStateChange:
                    break;
                case NetworkMessageType.Data:
                    break;
                /*
                case DemoConstants.EvPosition:
                    originatingPlayer.ReadEvMove((Hashtable)photonEvent[ParameterCode.CustomEventContent]);
                    break;
                case DemoConstants.EvColor:
                    originatingPlayer.ReadEvColor((Hashtable)photonEvent[ParameterCode.CustomEventContent]);
                    break;

                    // in this demo, we want a callback when players join or leave (so we can update their representation)
                case LiteEventCode.Join:
                    if (OnEventJoin != null) 
                    {
                        OnEventJoin(originatingPlayer);
                    }
                    break;
                case LiteEventCode.Leave:
                    if (OnEventLeave != null) 
                    {
                        OnEventLeave(originatingPlayer);
                    }
                    break;
                */
            }
        }

        public override void OnOperationResponse(OperationResponse operationResponse)
        {
            base.OnOperationResponse(operationResponse);  // important to call, to keep state up to date

            if (operationResponse.ReturnCode != ErrorCode.Ok)
            {
                //this.DebugReturn(DebugLevel.ERROR, operationResponse.ToStringFull() + " " + this.State);
            }

            // this demo connects when you call start and then it automatically executes a certain operation workflow to get you in a room
            switch (operationResponse.OperationCode)
            {
                case OperationCode.GetLobbyStats:
                    break;
                case OperationCode.JoinLobby:
                    break;
                case OperationCode.Authenticate:
                    // Unlike before, the game-joining can now be triggered by the simpler OnStateChangeAction delegate: OnStateChanged(ClientState newState)
                    break;

                case OperationCode.JoinRandomGame:
                    // OpJoinRandomRoom is called above. the response to that is handled here
                    // if the Master Server didn't find a room, simply create one. the result is handled below
                    //if (this.JoinRandomGame && operationResponse.ReturnCode != ErrorCode.Ok)
                    //{
                        //this.CreateParticleDemoRoom(DemoConstants.MapType.Forest, 4);
                    //}
                    break;

                case OperationCode.JoinGame:
                case OperationCode.CreateGame:
                    if (this.State == ClientState.JoinedLobby) {
                    }
                    // the master server will respond to join and create but this is handled in the base class
                    if (this.State == ClientState.Joined)
                    {
                        System.Diagnostics.Debug.WriteLine("Joined");
                    }
                    break;
            }
        }

        public override void OnStatusChanged(StatusCode statusCode)
        {
            base.OnStatusChanged(statusCode);  // important to call, to keep state up to date

            if (statusCode == StatusCode.Disconnect && this.DisconnectedCause != DisconnectCause.None)
            {
                DebugReturn(DebugLevel.ERROR, this.DisconnectedCause + " caused a disconnect. State: " + this.State + " statusCode: " + statusCode + ".");
            }
        }

        public void Start(bool joinLobby = false)
        {
            if (!joinLobby)
                AutoJoinLobby = true;

            PlayerName = SignedInGamer.SignedInGamers[0].Gamertag;
            if (!this.Connect())
            {
                this.DebugReturn(DebugLevel.ERROR, "Can't connect to MasterServer: " + this.MasterServerAddress);
            }
            var ev = new ManualResetEvent(false);
            Task.Run(() =>
                {
                    while (State != (joinLobby ? ClientState.ConnectedToMaster : ClientState.JoinedLobby)) {
                        Update();
                    }
                    ev.Set ();
                });
            ev.WaitOne();
            if (joinLobby)
            {
                ev.Reset();
                OpJoinLobby(lobby);
                Task.Run(() =>
                    {
                        while (State != ClientState.JoinedLobby) {
                            Update();
                        }
                        int i = 0;
                        while (i < 1000) {
                            Update ();
                            Thread.Sleep(10);
                            i++;
                        }
                        ev.Set ();
                    });
                ev.WaitOne();
            }
        }

        public void JoinSession(AvailableNetworkSession session)
        {
            OpJoinRoom(session.HostGamertag);
        }

        public void CreateSession (Gamer gamer, NetworkSessionProperties properties)
        {
            Start();
            if (IsConnectedAndReady)
            {
                var id = gamer.Gamertag;
                var opt = new RoomOptions() {
                    MaxPlayers = 4,
                    IsVisible = true,
                    IsOpen = true,
                };
                var visible = new List<string>();
                for (int i=0; i < properties.Count; i++)
                {
                    var p = properties[i];
                    if (p.HasValue)
                    {
                        opt.CustomRoomProperties.Add(i.ToString(), p.Value);
                        visible.Add(i.ToString());
                    }
                }
                opt.CustomRoomPropertiesForLobby = visible.ToArray();
                if (OpJoinOrCreateRoom(id, opt, lobby))
                {
                    while (CurrentRoom == null)
                    {
                        Update();
                    }
                }
            }
        }

        public static void FindRooms (IList<AvailableNetworkSession> sessions)
        {
            PhotonClient client = new PhotonClient();
            client.EnableLobbyStatistics = true;
            client.Start(true);
            if (client.IsConnectedAndReady) {
                var info = client.LobbyStatistics.Find(x => x.Name == "Master");
                var c = info.RoomCount;
                var rooms = client.RoomInfoList;
                foreach (var room in rooms)
                {
                    var a = new AvailableNetworkSession();
                    a.CurrentGamerCount = room.Value.PlayerCount;
                    a.HostGamertag = room.Value.Name;
                    a.OpenPrivateGamerSlots = !room.Value.IsOpen
                        ? room.Value.MaxPlayers - a.CurrentGamerCount
                        : 0;
                    a.OpenPublicGamerSlots = room.Value.IsOpen
                        ? room.Value.MaxPlayers - a.CurrentGamerCount
                        : 0;
                    a.SessionProperties = new NetworkSessionProperties();
                    foreach (var key in room.Value.CustomProperties.Keys)
                    {
                        int k;
                        if (!int.TryParse((string)key, out k))
                            continue;
                        var value = room.Value.CustomProperties[key];
                        int v;
                        if (value != null && int.TryParse ((string)value, out v))
                        {
                            a.SessionProperties[k] = v;
                        }
                    }
                    a.SessionType = NetworkSessionType.PlayerMatch;
                    sessions.Add(a);
                }

            }
        }
    }
}

