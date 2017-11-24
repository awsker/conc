using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace conc.game.input
{
    public class MouseInput : IInputDevice
    {
        public IEnumerable<ControlButtons> GetButtonsDown()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                yield return ControlButtons.FireRope;
        }
    }
}