using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Net;

namespace MonoGameServer.Packets
{
    public class Packet
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public PacketType PacketType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PacketDirection PacketDirection { get; set; }

        public long TimeStamp { get; set; }

        public Packet(long timeStamp)
        {
            TimeStamp = timeStamp;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static T Deserialize<T>(string json) where T : Packet
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }

    public enum PacketType
    {
        ConnectPacket,
        AssignPlayerPacket,
        DisconnectPacket,
        MovePacket
    }

    public enum PacketDirection
    {
        ToServer,
        ToClient,
        ToBoth
    }
}
