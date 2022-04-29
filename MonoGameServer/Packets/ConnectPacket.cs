using Microsoft.Xna.Framework;
using MonoGameServer.Game;
using Newtonsoft.Json;
using System;

namespace MonoGameServer.Packets
{
    public class ConnectPacket : Packet
    {
        public string Name { get; set; }

        [JsonConstructor]
        public ConnectPacket(string name, long timeStamp) : base(timeStamp)
        {
            PacketType = PacketType.ConnectPacket;
            PacketDirection = PacketDirection.ToServer;
            Name = name;
        }
    }
}
