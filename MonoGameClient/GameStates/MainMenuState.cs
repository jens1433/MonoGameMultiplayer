using Microsoft.Xna.Framework;
using System;

namespace BaseProject.GameStates
{
    internal class MainMenuState : GameObjectList, IGameState
    {
        public MainMenuState()
        {
            var text = new TextGameObject("GameFont");
            text.Text = "MonoGameMultiplayer";
            text.Position = new Vector2(GameEnvironment.Screen.X / 2f - text.Size.X / 2f, 100);
            Add(text);
            var text2 = new TextGameObject("GameFont");
            text2.Text = "Press Any Key To Join Localhost";
            text2.Position = new Vector2(GameEnvironment.Screen.X / 2f - text2.Size.X / 2f, 124);
            Add(text2);
        }

        public void EnterState()
        {
        }

        public void ExitState()
        {
        }

        public override void HandleInput(InputHelper inputHelper)
        {
            if (inputHelper.AnyKeyPressed)
            {
                GameEnvironment.GameStateManager.SwitchTo(nameof(JoiningServerState));
            }
        }
    }
}
