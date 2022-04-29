using Microsoft.Xna.Framework;
using MonoGameServer.Game;
using System;

namespace MonoGameServer.Packets
{
    public class AssignPlayerPacket : Packet
    {
        public NetPlayer Player { get; set; }

        public int Id { get; set; }

        public AssignPlayerPacket(NetPlayer player, int id, long timeStamp) : base(timeStamp)
        {
            PacketType = PacketType.AssignPlayerPacket;
            PacketDirection = PacketDirection.ToClient;
            Player = player;
            Id = id;
        }
    }
}
