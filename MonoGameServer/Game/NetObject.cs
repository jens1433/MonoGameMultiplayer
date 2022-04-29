using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace MonoGameServer.Game
{
    public class NetObject
    {
        public int Id { get; set; }

        [JsonConstructor]
        public NetObject(int id)
        {
            Id = id;
        }
    }
}
