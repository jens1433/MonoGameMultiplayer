using BaseProject.GameObjects;
using Microsoft.Xna.Framework;
using System;

namespace BaseProject.States
{
    internal class PlayerRunState : BaseState<Player>
    {
        public PlayerRunState(Player context, BaseStateFactory<Player> factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            context.Animator.PlayAnimation("Run");
        }

        public override void CheckSwitchState(InputHelper inputHelper)
        {
            if (!inputHelper.IsKeyDown(context.GetKeyMoveLeft()) && !inputHelper.IsKeyDown(context.GetKeyMoveRight()))
            {
                SwitchState(State.Idle);
            }

            if (!inputHelper.IsKeyDown(context.GetKeyRun()))
            {
                SwitchState(State.Walk);
            }

            if (inputHelper.IsKeyDown(context.GetKeyJump()))
            {
                SwitchState(State.Jump);
            }
        }
    }
}
