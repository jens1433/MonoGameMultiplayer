using Microsoft.Xna.Framework;
using System;

namespace BaseProject.GameStates
{
    internal class DisconnectedState : GameObjectList
    {
        TextGameObject text;

        public DisconnectedState()
        {
            Add(text = new TextGameObject("GameFont"));
        }

        public void SetText(string text)
        {
            this.text.Text = text;
        }
    }
}
