using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Components
{
    /// <summary>
    /// SpriteGameObject that handles rotated sprites. Overrides Draw method WITHOUT call to base.Draw.
    /// </summary>
    class RotatingComponent : Component
    {
        protected GameObject targetObject;
        protected float offsetDegrees;

        private float radians;

        public RotatingComponent(GameObject gameObject) : base(gameObject)
        {
        }

        /// <summary>
        /// Returns unit vector2 (length = 1) based on current angle.
        /// </summary>
        public Vector2 AngularDirection
        {
            get
            {
                // calculate angular direction based on sprite angle 
                return new Vector2((float)Math.Cos(Angle), (float)Math.Sin(Angle));
            }
            set
            {
                Angle = (float)Math.Atan2(value.Y, value.X);
            }
        }

        /// <summary>
        /// Returns / sets angle in radians (0 - 2*PI)
        /// </summary>
        public float Angle
        {
            get { return radians; }
            set { radians = value; }
        }

        /// <summary>
        /// Returns / sets angle in degrees (0 - 360)
        /// </summary>
        public float Degrees
        {
            get { return MathHelper.ToDegrees(Angle); }
            set { Angle = MathHelper.ToRadians(value); }
        }

        /// <summary>
        /// Sets the target object so this object will always be pointing towards the target object
        /// </summary>
        /// <param name="targetObject">GameObject to look at</param>
        /// <param name="offsetDegrees">degrees to offset from calculated angle</param>
        public void LookAt(GameObject targetObject, float offsetDegrees = 0)
        {
            this.targetObject = targetObject;
            this.offsetDegrees = offsetDegrees;
        }

        /// <summary>
        /// Clears the targetObject so it won't point towards it anymore
        /// </summary>
        public void StopLookingAtTarget()
        {
            targetObject = null;
            this.offsetDegrees = 0;
        }

        /// <summary>
        /// Updates the angle based on the position and the position of the target object
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (targetObject != null)
            {
                Vector2 targetVector = targetObject.GlobalPosition - GameObject.GlobalPosition;
                AngularDirection = targetVector;
            }

            base.Update(gameTime);
        }
    }
}