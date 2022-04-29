using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace BaseProject.States
{
    public class BaseState<T> where T : IHasState<T>
    {
        protected T context;
        protected BaseStateFactory<T> factory;

        public State StateType
        {
            get { return factory.States.FirstOrDefault(state => state.Value == this).Key; }
        }

        public BaseState(T context, BaseStateFactory<T> factory)
        {
            this.context = context;
            this.factory = factory;
        }

        public void SwitchState(State state)
        {
            ExitState();
            context.CurrentState = factory.States[state];
            context.CurrentState.EnterState();
        }

        public virtual void EnterState()
        {
        }

        public virtual void CheckSwitchState(InputHelper inputHelper)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }

        public virtual void ExitState() 
        { 
        }
    }
}
