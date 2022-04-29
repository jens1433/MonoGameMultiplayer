using BaseProject.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace BaseProject.States
{
    internal class PlayerIdleState : BaseState<Player>
    {
        public PlayerIdleState(Player context, PlayerStateFactory factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            context.Animator.PlayAnimation("Idle");
        }

        public override void CheckSwitchState(InputHelper inputHelper)
        {
            if (inputHelper.IsKeyDown(context.GetKeyMoveLeft()) || inputHelper.IsKeyDown(context.GetKeyMoveRight()))
            {
                if (inputHelper.IsKeyDown(context.GetKeyRun()))
                {
                    SwitchState(State.Run);
                }
                else
                {
                    SwitchState(State.Walk);
                }
            }

            if (inputHelper.KeyPressed(context.GetKeyJump()))
            {
                SwitchState(State.Jump);
            }
        }
    }
}
