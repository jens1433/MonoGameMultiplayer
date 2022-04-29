using BaseProject.GameObjects;
using Microsoft.Xna.Framework;
using System;

namespace BaseProject.States
{
    internal class PlayerAttackState : BaseState<Player>
    {
        private int comboCounter;

        public PlayerAttackState(Player context, BaseStateFactory<Player> factory) : base(context, factory)
        {
        }

        public override void EnterState()
        {
            int anim = 1;
            if (Math.Abs(context.Velocity.X) > 1)
            {
                anim = 3;
            }
            context.Animator.PlayAnimation("Attack" + anim);
            comboCounter = 1;
        }

        public override void CheckSwitchState(InputHelper inputHelper)
        {
            if (context.Animator.Current.AnimationEnded)
            {
                if (inputHelper.IsKeyDown(context.GetKeyAttack()))
                {
                    if (inputHelper.IsKeyDown(context.GetKeyMoveLeft()) || inputHelper.IsKeyDown(context.GetKeyMoveRight()))
                    {
                        context.Animator.PlayAnimation("Attack3");
                        return;
                    }

                    comboCounter++;
                    if (comboCounter > 2)
                    {
                        comboCounter = 1;
                    }
                    context.Animator.PlayAnimation("Attack" + comboCounter);
                    return;
                }

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

                if (inputHelper.IsKeyDown(context.GetKeyJump()))
                {
                    SwitchState(State.Jump);
                }
            }
        }
    }
}
