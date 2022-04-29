using BaseProject.GameObjects;
using Microsoft.Xna.Framework;
using System;

namespace BaseProject.States
{
    internal class PlayerLandState : BaseState<Player>
    {
        public PlayerLandState(Player context, BaseStateFactory<Player> factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            context.Animator.PlayAnimation("Land");
        }

        public override void CheckSwitchState(InputHelper inputHelper)
        {
            if (context.Animator.Current.AnimationEnded)
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
                else
                {
                    SwitchState(State.Idle);
                }

                if (inputHelper.KeyPressed(context.GetKeyJump()))
                {
                    SwitchState(State.Jump);
                }
            }
        }
    }
}
