using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace MonoGameServer.Packets
{
    public class MovePacket : Packet
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public int Id { get; set; }

        [JsonConstructor]
        public MovePacket(Vector2 position, Vector2 velocity, int id, long timeStamp) : base(timeStamp)
        {
            PacketType = PacketType.MovePacket;
            PacketDirection = PacketDirection.ToBoth;

            Position = position;
            Velocity = velocity;
            Id = id;
        }
    }
}
