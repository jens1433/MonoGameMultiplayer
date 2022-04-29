using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

internal interface IKeyMapping
{
    public Keys GetKeyMoveLeft();
    public Keys GetKeyMoveRight();
    public Keys GetKeyJump();
    public Keys GetKeyRun();
    public Keys GetKeyAttack();
}

