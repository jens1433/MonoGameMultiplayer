using BaseProject.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameServer.Packets;
using System;

namespace BaseProject
{
    public class Game1 : GameEnvironment
    {      
        protected override void LoadContent()
        {
            base.LoadContent();

            screen = new Point(800, 600);
            ApplyResolutionSettings();

            GameStateManager.AddGameState(nameof(MainMenuState), new MainMenuState());
            GameStateManager.AddGameState(nameof(JoiningServerState), new JoiningServerState());
            GameStateManager.AddGameState(nameof(PlayingState), new PlayingState());
            GameStateManager.AddGameState(nameof(DisconnectedState), new DisconnectedState());
            GameStateManager.SwitchTo(nameof(MainMenuState));
            
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            if (GameClient != null)
            {
                GameClient.SendPacket(new DisconnectPacket("Application exit", GameClient.ClientId, GameClient.EpochNow));
            }
        }

    }
}
