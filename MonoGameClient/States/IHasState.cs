using System;
using System.Collections.Generic;
using System.Text;

namespace BaseProject.States
{
    public interface IHasState<T> where T : IHasState<T>
    {
        public BaseState<T> CurrentState { set; get; }
    }
}
