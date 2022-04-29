using Components;
using Microsoft.Xna.Framework;
using System;

namespace BaseProject.GameObjects
{
    internal class Platform : GameObject
    {
        protected ColliderComponent colliderComponent;
        private static int idCounter = 0;

        public ColliderComponent ColliderComponent
        {
            get { return colliderComponent; }
        }

        public Platform()
        {
            id = idCounter++.ToString();
            var sprite = AddComponent(new SpriteComponent("World/grassthing", this));
            colliderComponent = AddComponent(new ColliderComponent(new Rectangle(0, 0, sprite.Width, sprite.Height), this));
        }
    }
}
