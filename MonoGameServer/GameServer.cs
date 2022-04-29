using Microsoft.Xna.Framework;
using NetCoreServer;
using System;
using System.Net;
using MonoGameServer.Game;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using MonoGameServer.Packets;

namespace MonoGameServer
{
    internal class GameServer : UdpServer
    {
        private int idCounter;

        private ServerConfig config;

        List<ServerPlayer> players = new List<ServerPlayer>();
        List<NetObject> netObjects = new List<NetObject>();

        bool isReceiving = false;

        public static long EpochNow
        {
            get { return DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond; }
        }

        public GameServer(int port) : base(IPAddress.Any, port)
        {
            config = new ServerConfig();
            config.SetValue("MaxPlayerCount", 32);
            config.SetValue("MaxPlayerSpeed", 2100f);
        }

        protected override void OnStarted()
        {
            Console.WriteLine($"Server started at port {Endpoint.Port}");
            ReceiveShit();
        }

        protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
        {
            isReceiving = false;
            if (endpoint.AddressFamily == AddressFamily.InterNetwork)
            {
                IPEndPoint ipEndPoint = endpoint as IPEndPoint;
                string msg = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);

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
                    Console.WriteLine($"Received from {ipEndPoint.Address}: " + p);

                    var packet = Packet.Deserialize<Packet>(p);
                    if (packet.PacketDirection == PacketDirection.ToClient)
                    {
                        return;
                    }
                    PacketType packetType = packet.PacketType;

                    switch (packetType)
                    {
                        case PacketType.ConnectPacket:
                            OnReceiveConnectPacket(Packet.Deserialize<ConnectPacket>(p), ipEndPoint);
                            break;
                        case PacketType.DisconnectPacket:
                            OnReceiveDisconnectPacket(Packet.Deserialize<DisconnectPacket>(p), ipEndPoint);
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
        }

        private void ReceiveShit()
        {
            if(!isReceiving)
            {
                ReceiveAsync();
                isReceiving = true;
            }
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Server caught an error with code {error}");
        }

        public override bool Stop()
        {
            foreach (var player in players)
            {
                SendPacket(new DisconnectPacket("Server closed", player.Player.Id, EpochNow), player.IpEndPoint);
            }
            players.Clear();

            return base.Stop();
        }

        private void SendPacket(Packet packet, IPEndPoint ipEndPoint)
        {
            SendAsync(ipEndPoint, packet.Serialize());

            string s = "";

            if (FindPlayerByAddress(ipEndPoint, out var sPlayer))
            {
                s = $"client {sPlayer.Player.Id} at";
            }

            Console.WriteLine($"Send {packet.GetType().Name} to {s} {ipEndPoint.Address}");
        }

        private void OnReceiveConnectPacket(ConnectPacket packet, IPEndPoint ipEndPoint)
        {
            if (players.Count >= config.GetValue<int>("MaxPlayerCount"))
            {
                SendPacket(new DisconnectPacket("Server is full", 0, EpochNow), ipEndPoint);
                return;
            }
            var netPlayer = new NetPlayer(packet.Name, 0, new Vector2(100, 100), new Vector2(), idCounter++);
            netObjects.Add(netPlayer);
            var serverPlayer = new ServerPlayer(ipEndPoint, netPlayer);
            serverPlayer.AddPacketTime(PacketType.ConnectPacket, packet.TimeStamp);

            var p = new AssignPlayerPacket(netPlayer, netPlayer.Id, EpochNow);
            SendPacket(p, ipEndPoint);
            foreach (var player in players)
            {
                SendPacket(new AssignPlayerPacket(player.Player, netPlayer.Id, EpochNow), ipEndPoint);
                SendPacket(new AssignPlayerPacket(netPlayer, player.Player.Id, EpochNow), player.IpEndPoint);
            }
            players.Add(serverPlayer);
        }

        private void OnReceiveDisconnectPacket(DisconnectPacket packet, IPEndPoint ipEndPoint)
        {
            Console.WriteLine($"Client {packet.Id} at {ipEndPoint.Address} disconnected: " + packet.Reason);
            players.Remove(FindPlayerById(packet.Id));
            packet.TimeStamp = EpochNow;
            foreach (var player in players)
            {
                if (player.Player.Id == packet.Id)
                {
                    continue;
                }
                SendPacket(packet, player.IpEndPoint);
            }
        }

        private void OnReceiveMovePacket(MovePacket packet)
        {
            ServerPlayer sPlayer = FindPlayerById(packet.Id);
            if (DoVelocityCorrection(packet, sPlayer))
            {
                SendMovePacket(sPlayer);
            }

            foreach (var player in players)
            {
                if (player == sPlayer)
                {
                    continue;
                }

                SendMovePacket(sPlayer, player.IpEndPoint);
            }

            sPlayer.AddPacketTime(PacketType.MovePacket, packet.TimeStamp);
        }

        private bool DoVelocityCorrection(MovePacket packet, ServerPlayer serverPlayer)
        {
            float delta = (packet.TimeStamp - serverPlayer.GetLastPacketTime(PacketType.MovePacket)) / 1000f;
            if (delta > 0)
            {
                float distanceTraveled = (packet.Position - serverPlayer.Player.Position).Length();
                float averageSpeed = distanceTraveled * delta;
                float maxSpeed = config.GetValue<float>("MaxPlayerSpeed");
                bool appliedCorrection = false;
                if (averageSpeed < maxSpeed)
                {
                    serverPlayer.Player.Position = packet.Position;
                }
                else
                {
                    appliedCorrection = true;
                }
                if (packet.Velocity.Length() < maxSpeed)
                {
                    serverPlayer.Player.Velocity = packet.Velocity;
                }
                else
                {
                    serverPlayer.Player.Velocity *= maxSpeed / packet.Velocity.Length();
                    appliedCorrection = true;
                }

                return appliedCorrection;
            }
            return false;
        }

        private void SendMovePacket(ServerPlayer player, IPEndPoint endPoint = null)
        {
            if (endPoint == null)
            {
                endPoint = player.IpEndPoint;
            }
            SendPacket(new MovePacket(player.Player.Position, player.Player.Velocity, player.Player.Id, EpochNow), endPoint);
        }

        private ServerPlayer FindPlayerById(int id)
        {
            return players.FirstOrDefault(serverPlayer => serverPlayer.Player.Id == id);
        }

        private ServerPlayer FindPlayerByAddress(IPEndPoint ipEndPoint)
        {
            return players.FirstOrDefault(serverPlayer => serverPlayer.IpEndPoint == ipEndPoint);
        }

        private bool FindPlayerByAddress(IPEndPoint ipEndPoint, out ServerPlayer player)
        {
            player = players.FirstOrDefault(serverPlayer => serverPlayer.IpEndPoint == ipEndPoint);
            return player != null;
        }
    }
}
