using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Components
{
    public class SpriteComponent : Component
    {
        protected Color shade = Color.White;
        protected SpriteSheet sprite;
        protected Vector2 origin;
        protected float scale = 1f;
        protected float mirrorXOffset = 0.0f;

        public SpriteComponent(string assetName, GameObject gameObject) : base(gameObject)
        {
            if (assetName != "")
            {
                sprite = new SpriteSheet(assetName);
            }
            else
            {
                sprite = null;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!GameObject.Visible)
            {
                return;
            }

            var _sprite = sprite;
            { // Extra scope so component goes out of scope after the if statement.
                if (GameObject.GetComponent<AnimatedComponent>(out var spriteComponent))
                {
                    _sprite = spriteComponent.Current;
                }
            }
            if (_sprite == null)
            {
                return;
            }

            var rotation = 0.0f;
            {
                if (GameObject.GetComponent<RotatingComponent>(out var rotatingComponent))
                {
                    rotation = rotatingComponent.Angle;
                }
            }

            var pos = GameObject.GlobalPosition;
            if (Mirror)
            {
                pos -= new Vector2(mirrorXOffset, 0);
            }

            _sprite.Draw(spriteBatch, pos, Origin, rotation, scale, shade);
        }

        public SpriteSheet Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        public Vector2 Center
        {
            get { return new Vector2(Width, Height) / 2; }
        }

        public int Width
        {
            get
            {
                return sprite.Width;
            }
        }

        public int Height
        {
            get
            {
                return sprite.Height;
            }
        }

        /// <summary>
        /// Returns / sets the scale of the sprite.
        /// </summary>
        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// Set the shade the sprite will be drawn in.
        /// </summary>
        public Color Shade
        {
            get { return shade; }
            set { shade = value; }
        }

        public bool Mirror
        {
            get { return sprite.Mirror; }
            set { sprite.Mirror = value; }
        }

        public float MirrorXOffset
        {
            get { return mirrorXOffset; }
            set { mirrorXOffset = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        /*    public Rectangle BoundingBox
            {
                get
                {
                    int left = (int)(GameObject.GlobalPosition.X - origin.X);
                    int top = (int)(GameObject.GlobalPosition.Y - origin.Y);
                    return new Rectangle(left, top, Width, Height);
                }
            }*/
    }
}