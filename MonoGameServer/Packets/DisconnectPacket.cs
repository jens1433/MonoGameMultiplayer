using Microsoft.Xna.Framework;
using System;

namespace MonoGameServer.Packets
{
    public class DisconnectPacket : Packet
    {
        public string Reason { get; set; }

        public int Id { get; set; }

        public DisconnectPacket(string reason, int id, long timeStamp) : base(timeStamp)
        {
            PacketType = PacketType.DisconnectPacket;
            PacketDirection = PacketDirection.ToBoth;

            Reason = reason;
            Id = id;
        }
    }
}
