using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace conc.game.input
{
    public class KeyboardInput : IInputDevice
    {
        public IEnumerable<ControlButtons> GetButtonsDown()
        {
            foreach (var keybind in Keybinds)
            {
                if (Keyboard.GetState().IsKeyDown(keybind.Value))
                    yield return keybind.Key;
            }

            //if (Keyboard.GetState().IsKeyDown(Keys.Up))
            //    yield return ControlButtons.MoveUp;

            //if (Keyboard.GetState().IsKeyDown(Keys.Down))
            //    yield return ControlButtons.MoveDown;

            //if (Keyboard.GetState().IsKeyDown(Keys.Left))
            //    yield return ControlButtons.MoveLeft;

            //if (Keyboard.GetState().IsKeyDown(Keys.Right))
            //    yield return ControlButtons.MoveRight;

            //if (Keyboard.GetState().IsKeyDown(Keys.RightControl))
            //    yield return ControlButtons.Jump;

            //if (Keyboard.GetState().IsKeyDown(Keys.RightShift) || Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            //    yield return ControlButtons.FireRope;

            //if (Keyboard.GetState().IsKeyDown(Keys.W))
            //    yield return ControlButtons.MoveUp;

            //if (Keyboard.GetState().IsKeyDown(Keys.S))
            //    yield return ControlButtons.MoveDown;

            //if (Keyboard.GetState().IsKeyDown(Keys.A))
            //    yield return ControlButtons.MoveLeft;

            //if (Keyboard.GetState().IsKeyDown(Keys.D))
            //    yield return ControlButtons.MoveRight;

            //if (Keyboard.GetState().IsKeyDown(Keys.Space))
            //    yield return ControlButtons.Jump;
        }

        public Dictionary<ControlButtons, Keys> Keybinds { get; set; } = new Dictionary<ControlButtons, Keys>();
    }
}
