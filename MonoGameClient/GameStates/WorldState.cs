using BaseProject.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameServer.Game;
using System;
using System.Collections.Generic;

namespace BaseProject.GameStates
{
    internal class WorldState : GameObjectList, IGameState
    {
        private CollisionManager collisionManager;
        private List<NetPlayer> netPlayerQueue = new List<NetPlayer>();
        private NetPlayer localNetPlayer;

        public WorldState()
        {
            collisionManager = new CollisionManager();
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            foreach (var netPlayer in netPlayerQueue)
            {
                Add(Player.FromNetPlayer(netPlayer, true));
            }
            netPlayerQueue.Clear();
            if (localNetPlayer != null)
            {
                Add(Player.FromNetPlayer(localNetPlayer, false));
                localNetPlayer = null;
            }

            base.HandleInput(inputHelper);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            collisionManager.CheckCollision(Children);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            collisionManager.ClearCallList();
        }

        public void EnterState()
        {
        }

        public void ExitState()
        {
        }

        public void AddPlayer(NetPlayer netPlayer, bool isReplicated)
        {
            if (isReplicated)
            {
                netPlayerQueue.Add(netPlayer);
            }
            else
            {
                localNetPlayer = netPlayer;
            }
        }
    }
}
