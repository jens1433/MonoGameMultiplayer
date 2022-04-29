using BaseProject.GameObjects;
using Microsoft.Xna.Framework;
using System;

namespace BaseProject.States
{
    internal class PlayerFallState : BaseState<Player>
    {
        public PlayerFallState(Player context, BaseStateFactory<Player> factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            context.Animator.PlayAnimation("Fall");
        }

        public override void CheckSwitchState(InputHelper inputHelper)
        {
            if(context.Velocity.Y <= 0)
            {
                SwitchState(State.Land);
            }
        }
    }
}
