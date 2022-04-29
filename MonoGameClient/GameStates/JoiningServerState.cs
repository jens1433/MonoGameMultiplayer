using BaseProject.GameObjects;
using Microsoft.Xna.Framework;
using MonoGameServer.Packets;
using System;

namespace BaseProject.GameStates
{
    internal class JoiningServerState : GameObjectList, IGameState
    {
        TextGameObject text;

        public JoiningServerState()
        {
            Add(text = new TextGameObject("GameFont") { Text = "Joining Server" });
        }

        public void EnterState()
        {
            GameEnvironment.CreateClient("83.128.7.208", 1111);
            text.Text = "Connecting...";
            GameEnvironment.GameClient.Connect();
            GameEnvironment.GameClient.SendPacket(new ConnectPacket("Test", GameClient.EpochNow));
        }

        public void ExitState()
        {
        }

        public void OnReceiveAssignPlayerPacket(AssignPlayerPacket packet)
        {
            GameEnvironment.GameStateManager.SwitchTo(nameof(PlayingState));
            if (GameEnvironment.GameStateManager.CurrentGameState is PlayingState playingState)
            {
                playingState.AddPlayer(packet.Player, packet.Id != packet.Player.Id);
            }
        }
    }
}
