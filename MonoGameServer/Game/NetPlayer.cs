using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace MonoGameServer.Game
{
    public class NetPlayer : NetEntity
    {
        public string Name { get; set; }
        public int State { get; set; }

        [JsonIgnore]
        public object Player { get; set; }

        [JsonConstructor]
        public NetPlayer(string name, int state, Vector2 position, Vector2 velocity, int id) : base(position, velocity, id)
        {
            Name = name;
            State = state;
        }
    }
}
