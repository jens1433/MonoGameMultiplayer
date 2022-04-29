using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;

namespace MonoGameServer.Game
{
    public class NetEntity : NetObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        [JsonConstructor]
        public NetEntity(Vector2 position, Vector2 velocity, int id) : base(id)
        {
            Position = position;
            Velocity = velocity;
        }

        public void SetMovement(Vector2 position, Vector2 velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }
}
