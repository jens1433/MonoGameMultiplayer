using BaseProject.GameObjects;
using Microsoft.Xna.Framework;
using System;

namespace BaseProject.States
{
    public class PlayerStateFactory : BaseStateFactory<Player>
    {
        public PlayerStateFactory(Player context) : base(context)
        {
            States.Add(State.Idle, new PlayerIdleState(context, this));
            States.Add(State.Walk, new PlayerWalkState(context, this));
            States.Add(State.Run, new PlayerRunState(context, this));
            States.Add(State.Jump, new PlayerJumpState(context, this));
            States.Add(State.Fall, new PlayerFallState(context, this));
            States.Add(State.Land, new PlayerLandState(context, this));
            States.Add(State.Attack, new PlayerAttackState(context, this));
        }
    }
}
