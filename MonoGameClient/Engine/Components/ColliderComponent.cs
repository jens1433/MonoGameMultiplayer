using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Components
{
    public class ColliderComponent : Component
    {
        private Rectangle boundingBox;

        public Rectangle BoundingBox
        {
            get
            {
                Vector2 origin = Vector2.Zero;

                if (GameObject.GetComponent<SpriteComponent>(out var sprite))
                {
                    origin = sprite.Origin;
                }

                return new Rectangle(
                    (int)Math.Round(GameObject.GlobalPosition.X - origin.X + boundingBox.X),
                    (int)Math.Round(GameObject.GlobalPosition.Y - origin.Y + boundingBox.Y),
                    boundingBox.Width,
                    boundingBox.Height
                );
            }
        }

        public ColliderComponent(Rectangle boundingBox, GameObject gameObject) : base(gameObject)
        {
            this.boundingBox = boundingBox;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (GameEnvironment.IsDebug)
            {
                DrawingHelper.DrawRectangle(BoundingBox, spriteBatch, Color.MediumPurple);
            }
        }
    }
}
