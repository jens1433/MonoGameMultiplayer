using BaseProject.States;
using Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameServer.Game;
using System;

namespace BaseProject.GameObjects
{
    public class Player : Entity, IHasState<Player>, IKeyMapping
    {
        protected AnimatedComponent animator;
        protected PlayerStateFactory stateFactory;

        protected string name;
        protected NetPlayer netPlayer;

        private const int exportScale = 3;
        private const float walkAcceleration = 2000;
        private const float runAcceleration = 5000;
        private const float jumpForce = 1300;

        public BaseState<Player> CurrentState { get; set; }

        public AnimatedComponent Animator
        {
            get { return animator; }
        }

        public string Name
        {
            get { return name; }
        }

        public Player() : base("", new Rectangle(8 * exportScale, 15 * exportScale, 17 * exportScale, 33 * exportScale))
        {
            AddComponent(animator = new AnimatedComponent(this));
            LoadAnimations();

            stateFactory = new PlayerStateFactory(this);
            CurrentState = stateFactory.States[State.Idle];
            CurrentState.EnterState();
        }

        public static Player FromNetPlayer(NetPlayer netPlayer, bool isReplicated)
        {
            var player = new Player()
            {
                position = netPlayer.Position,
                velocity = netPlayer.Velocity,
                name = netPlayer.Name
            };
            player.CurrentState = player.stateFactory.States[(State)netPlayer.State];
            player.CurrentState.EnterState();
            player.isReplicated = isReplicated;
            player.netPlayer = netPlayer;
            netPlayer.Player = player;
            return player;
        }

        private void LoadAnimations()
        {
            animator.LoadAnimation("Character/GraveRobber_idle@4", "Idle", true, 0.2f);
            animator.LoadAnimation("Character/GraveRobber_run@6", "Run", true);
            animator.LoadAnimation("Character/GraveRobber_walk@6", "Walk", true, 0.15f);
            animator.LoadAnimation("Character/GraveRobber_jump@3", "Jump", false);
            animator.LoadAnimation("Character/GraveRobber_fall@1", "Fall", false);
            animator.LoadAnimation("Character/GraveRobber_land@2", "Land", false);
            animator.LoadAnimation("Character/GraveRobber_attack1@6", "Attack1", false);
            animator.LoadAnimation("Character/GraveRobber_attack2@6", "Attack2", false);
            animator.LoadAnimation("Character/GraveRobber_attack3@6", "Attack3", false);
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (isReplicated)
            {
                return;
            }

            base.HandleInput(inputHelper);

            float accel = walkAcceleration;
            if (inputHelper.IsKeyDown(GetKeyRun()))
            {
                accel = runAcceleration;
            }

            if (inputHelper.IsKeyDown(GetKeyMoveLeft()) && onGround)
            {
                acceleration.X = -accel;
            }
            else if (inputHelper.IsKeyDown(GetKeyMoveRight()) && onGround)
            {
                acceleration.X = accel;
            }
            else
            {
                acceleration.X = 0;
            }

            if (inputHelper.KeyPressed(GetKeyJump()) && onGround)
            {
                velocity.Y = -jumpForce;
            }

            if (inputHelper.KeyPressed(GetKeyAttack()) && onGround && CurrentState != stateFactory.States[State.Attack])
            {
                CurrentState.SwitchState(State.Attack);
            }

            CurrentState.CheckSwitchState(inputHelper);

            //System.Diagnostics.Debug.WriteLine(CurrentState);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (velocity.X != 0)
                spriteComponent.Mirror = velocity.X < 0;

            CurrentState.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            CurrentState.Draw(gameTime, spriteBatch);
        }

        public void UpdateNetPlayer()
        {
            position = netPlayer.Position;
            velocity = netPlayer.Velocity;
        }

        public Keys GetKeyMoveLeft()
        {
            return Keys.A;
        }

        public Keys GetKeyMoveRight()
        {
            return Keys.D;
        }

        public Keys GetKeyJump()
        {
            return Keys.Space;
        }

        public Keys GetKeyRun()
        {
            return Keys.LeftShift;
        }

        public Keys GetKeyAttack()
        {
            return Keys.E;
        }
    }
}
