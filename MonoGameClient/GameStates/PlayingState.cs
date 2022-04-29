using BaseProject.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace BaseProject.GameStates
{
    internal class PlayingState : WorldState
    {
        public PlayingState()
        {
            Add(new Platform() { Position = new Vector2(0, 350) });
            Add(new Platform() { Position = new Vector2(500, 500) });
        }
    }
}
