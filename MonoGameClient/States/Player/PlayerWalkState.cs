using BaseProject.GameObjects;
using Microsoft.Xna.Framework;
using System;

namespace BaseProject.States
{
    internal class PlayerWalkState : BaseState<Player>
    {
        public PlayerWalkState(Player context, BaseStateFactory<Player> factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            context.Animator.PlayAnimation("Walk");
        }

        public override void CheckSwitchState(InputHelper inputHelper)
        {
            if (!inputHelper.IsKeyDown(context.GetKeyMoveLeft()) && !inputHelper.IsKeyDown(context.GetKeyMoveRight()))
            {
                SwitchState(State.Idle);
            }

            if(inputHelper.IsKeyDown(context.GetKeyRun()))
            {
                SwitchState(State.Run);
            }

            if (inputHelper.KeyPressed(context.GetKeyJump()))
            {
                SwitchState(State.Jump);
            }
        }
    }
}
