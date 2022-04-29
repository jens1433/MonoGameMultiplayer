using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BaseProject.States
{
    public class BaseStateFactory<T> where T : IHasState<T>
    {
        private T context;
        private Dictionary<State, BaseState<T>> states = new Dictionary<State, BaseState<T>>();

        public Dictionary<State, BaseState<T>> States
        {
            get { return states; }
        }

        public BaseStateFactory(T context)
        {
            this.context = context;
        }
    }

    public enum State
    {
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
        Land,
        Attack
    }
}
