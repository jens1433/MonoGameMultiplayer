using BaseProject.GameObjects;
using BaseProject.GameStates;
using Microsoft.Xna.Framework;
using MonoGameServer.Game;
using MonoGameServer.Packets;
using NetCoreServer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using UdpClient = NetCoreServer.UdpClient;

namespace BaseProject
{
    public class GameClient : UdpClient
    {
        private GameEnvironment gameEnvironment;
        private bool disconnectedByPacket;
        private List<NetPlayer> players = new List<NetPlayer>();

        bool isReceiving = false;

        public int ClientId { get; private set; }

        public GameClient(GameEnvironment gameEnvironment, string address, int port) : base(address, port)
        {
            this.gameEnvironment = gameEnvironment;
            ClientId = -1;
        }

        public static long EpochNow
        {
            get { return DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond; }
        }

        protected override void OnConnected()
        {
            System.Diagnostics.Debug.WriteLine("Connected");
            ReceiveShit();
        }

        protected override void OnDisconnected()
        {
            System.Diagnostics.Debug.WriteLine("Disconnected");
            if (!disconnectedByPacket)
            {
                GameEnvironment.GameStateManager.SwitchTo(nameof(DisconnectedState));
                if (GameEnvironment.GameStateManager.CurrentGameState is DisconnectedState state)
                {
                    state.SetText("Connection failed");
                }
            }
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            isReceiving = false;
            if (endpoint.AddressFamily == AddressFamily.InterNetwork)
            {
                IPEndPoint ipEndPoint = endpoint as IPEndPoint;
                string msg = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);

                System.Diagnostics.Debug.WriteLine($"Received from {ipEndPoint.Address}: " + msg);

                string[] packets = msg.Split("}{", StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < packets.Length; i++)
                {
                    if (i < packets.Length - 1)
                    {
                        packets[i] += "}";
                    }
                    if (i > 0)
                    {
                        packets[i] = "{" + packets[i];
                    }
                }

                foreach (string p in packets)
                {
                    var packet = Packet.Deserialize<Packet>(p);
                    if (packet.PacketDirection == PacketDirection.ToServer)
                    {
                        return;
                    }
                    PacketType packetType = packet.PacketType;

                    switch (packetType)
                    {
                        case PacketType.AssignPlayerPacket:
                            var apPacket = Packet.Deserialize<AssignPlayerPacket>(p);
                            players.Add(apPacket.Player);
                            if (GameEnvironment.GameStateManager.CurrentGameState is JoiningServerState jState)
                            {
                                if (apPacket.Player.Id == apPacket.Id)
                                {
                                    ClientId = apPacket.Id;
                                }
                                jState.OnReceiveAssignPlayerPacket(apPacket);
                            }
                            else if (GameEnvironment.GameStateManager.CurrentGameState is WorldState wState)
                            {
                                wState.AddPlayer(apPacket.Player, true);
                            }
                            break;
                        case PacketType.DisconnectPacket:
                            OnReceiveDisconnectPacket(Packet.Deserialize<DisconnectPacket>(p));
                            break;
                        case PacketType.MovePacket:
                            OnReceiveMovePacket(Packet.Deserialize<MovePacket>(p));
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Received: " + Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
            }
            ReceiveShit();
        }

        protected override void OnSent(EndPoint endpoint, long sent)
        {
            base.OnSent(endpoint, sent);
            ReceiveShit();
        }

        private void ReceiveShit()
        {
            if (!isReceiving)
            {
                ReceiveAsync();
                isReceiving = true;
            }
        }

        protected override void OnError(SocketError error)
        {
            System.Diagnostics.Debug.WriteLine($"Client caught an error with code {error}");
        }

        public void SendPacket(Packet packet)
        {
            if (packet is DisconnectPacket)
            {
                disconnectedByPacket = true;
            }
            SendAsync(packet.Serialize());
        }

        private void OnReceiveDisconnectPacket(DisconnectPacket packet)
        {
            if (packet.Id == ClientId)
            {
                disconnectedByPacket = true;
                GameEnvironment.GameStateManager.SwitchTo(nameof(DisconnectedState));
                if (GameEnvironment.GameStateManager.CurrentGameState is DisconnectedState state)
                {
                    state.SetText(packet.Reason);
                    Disconnect();
                }
            }
            else
            {
                NetPlayer player = GetNetPlayerById(packet.Id);
                if (player != null)
                {
                    players.Remove(GetNetPlayerById(packet.Id));
                    if (GameEnvironment.GameStateManager.CurrentGameState is PlayingState state)
                    {
                        state.Remove(player.Player as Player);
                    }
                }
            }
        }

        private void OnReceiveMovePacket(MovePacket packet)
        {
            NetPlayer p = GetNetPlayerById(packet.Id);
            p.SetMovement(packet.Position, packet.Velocity);
            if (p.Player is Player player)
            {
                player.UpdateNetPlayer();
            }
        }

        public void SendMovePacket(Entity entity)
        {
            var p = new MovePacket(entity.Position, entity.Velocity, ClientId, EpochNow);
            SendPacket(p);
        }

        private NetPlayer GetNetPlayerById(int id)
        {
            return players.FirstOrDefault(netPlayer => netPlayer.Id == id);
        }
    }
}
