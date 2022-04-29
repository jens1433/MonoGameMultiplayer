using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Components
{
    public class AnimatedComponent : Component
    {
        protected Dictionary<string, Animation> animations;
        private SpriteComponent spriteComp;

        public AnimatedComponent(GameObject gameObject) : base(gameObject)
        {
            animations = new Dictionary<string, Animation>();
            spriteComp = GameObject.GetComponent<SpriteComponent>();
        }

        public void LoadAnimation(string assetName, string id, bool looping,
                                  float frameTime = 0.1f)
        {
            Animation anim = new Animation(assetName, looping, frameTime);
            animations[id] = anim;
        }

        public void PlayAnimation(string id)
        {
            if (spriteComp.Sprite == animations[id] && !animations[id].AnimationEnded)
            {
                return;
            }
            if (spriteComp.Sprite != null)
            {
                animations[id].Mirror = spriteComp.Sprite.Mirror;
            }
            animations[id].Play();
            spriteComp.Sprite = animations[id];
            spriteComp.Origin = new Vector2(spriteComp.Sprite.Width / 2, spriteComp.Sprite.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (spriteComp.Sprite == null)
            {
                return;
            }
            Current.Update(gameTime);
            base.Update(gameTime);
        }

        public Animation Current
        {
            get { return spriteComp.Sprite as Animation; }
        }
    }
}