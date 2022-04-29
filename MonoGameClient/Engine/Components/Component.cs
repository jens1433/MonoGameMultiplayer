using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Components
{
    public abstract class Component : IGameLoopObject
    {
        private GameObject gameObject;

        public GameObject GameObject
        {
            get { return gameObject; }
        }

        public Component(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public virtual void HandleInput(InputHelper inputHelper)
        {
        }

        public virtual void Reset()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}