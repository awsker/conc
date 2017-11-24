using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace conc.game.input
{
    public class KeyboardInput : IInputDevice
    {
        public IEnumerable<ControlButtons> GetButtonsDown()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                yield return ControlButtons.Up;

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                yield return ControlButtons.Down;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                yield return ControlButtons.Left;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                yield return ControlButtons.Right;

            if (Keyboard.GetState().IsKeyDown(Keys.RightControl))
                yield return ControlButtons.Jump;

            if (Keyboard.GetState().IsKeyDown(Keys.RightShift) || Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                yield return ControlButtons.FireRope;

            if (Keyboard.GetState().IsKeyDown(Keys.W))
                yield return ControlButtons.Up;

            if (Keyboard.GetState().IsKeyDown(Keys.S))
                yield return ControlButtons.Down;

            if (Keyboard.GetState().IsKeyDown(Keys.A))
                yield return ControlButtons.Left;

            if (Keyboard.GetState().IsKeyDown(Keys.D))
                yield return ControlButtons.Right;

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                yield return ControlButtons.Jump;
        }
    }
}
