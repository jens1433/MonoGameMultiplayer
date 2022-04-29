using Components;
using Microsoft.Xna.Framework;
using System;

namespace BaseProject.GameObjects
{
    public class Entity : GameObject
    {
        protected Vector2 acceleration = Vector2.Zero;
        protected float mass = 0.0f;
        protected bool onGround = false;

        protected SpriteComponent spriteComponent;
        protected ColliderComponent colliderComponent;

        protected bool isReplicated;

        public bool IsReplicated
        {
            get { return isReplicated; }
        }

        public bool OnGround
        {
            get { return onGround; }
            set { onGround = value; }
        }

        public Entity(string assetName, Rectangle boundingBox)
        {
            spriteComponent = AddComponent(new SpriteComponent(assetName, this) { MirrorXOffset = 44.0f });
            colliderComponent = AddComponent(new ColliderComponent(boundingBox, this));
            mass = boundingBox.Width * boundingBox.Height;
        }

        public override void Update(GameTime gameTime)
        {
            var gravity = 0.0f;
            if(!isReplicated)
            {
                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (delta > 1f / 20f)
                {
                    delta = 1f / 60f;
                }
                // Ground resistance
                if (OnGround)
                {
                    velocity.X *= 0.8f;
                }
                velocity += acceleration * delta;
                // Gravity
                gravity = GameEnvironment.GravityConstant * mass * delta;
                velocity.Y += gravity;
                // Terminal velocity
                velocity *= Math.Min(1, (0.4f * mass) / velocity.Length());

                if (Math.Abs(velocity.X) < 10)
                {
                    velocity.X = 0;
                    GameEnvironment.GameClient.SendMovePacket(this);
                }
            }

            base.Update(gameTime);

            if(!isReplicated)
            {
                if (velocity.X != 0 || velocity.Y != gravity)
                {
                    GameEnvironment.GameClient.SendMovePacket(this);
                }
            }
        }

        public override void OnCollisionEnter(GameObject other)
        {
            base.OnCollisionEnter(other);

            if (other is Platform platform)
            {
                ResolvePlatformCollision(platform);
            }
        }

        public override void OnCollisionStay(GameObject other)
        {
            base.OnCollisionStay(other);

            if (other is Platform platform)
            {
                ResolvePlatformCollision(platform);
            }
        }

        private void ResolvePlatformCollision(Platform platform)
        {
            DirectionLengths delta;
            Rectangle bb = colliderComponent.BoundingBox;
            delta.Right = bb.X + bb.Width - platform.Position.X;
            delta.Left = bb.X - (platform.Position.X + platform.ColliderComponent.BoundingBox.Width);
            delta.Top = bb.Y + bb.Height - platform.Position.Y;
            delta.Bottom = bb.Y - (platform.Position.Y + platform.ColliderComponent.BoundingBox.Height);
            float closest = delta.GetShortest();
            if (delta.Top <= velocity.Y)
            {
                closest = delta.Top;
            }
            if (closest == delta.Left)
            {
                position.X -= delta.Left;
                velocity.X = 0;
            }
            else if (closest == delta.Right)
            {
                position.X -= delta.Right;
                velocity.X = 0;
            }
            else if (closest == delta.Top)
            {
                if (velocity.Y > 0)
                {
                    onGround = true;
                    position.Y -= delta.Top;
                    velocity.Y = 0;
                }
            }
            else if (closest == delta.Bottom)
            {
                position.Y -= delta.Bottom;
                if (velocity.Y < 0)
                {
                    velocity.Y = 0;
                }
            }
        }

        public override void OnCollisionExit(GameObject other)
        {
            base.OnCollisionExit(other);
            if (other is Platform)
            {
                onGround = false;
            }
        }
    }
}

internal struct DirectionLengths
{
    public float Left;
    public float Right;
    public float Top;
    public float Bottom;

    public float GetShortest()
    {
        float shortest = Left;
        if (Math.Abs(Right) < Math.Abs(shortest))
        {
            shortest = Right;
        }
        if (Math.Abs(Top) < Math.Abs(shortest))
        {
            shortest = Top;
        }
        if (Math.Abs(Bottom) < Math.Abs(shortest))
        {
            shortest = Bottom;
        }
        return shortest;
    }
}