using System;
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
                var key1 = keybind.Value.Item1;
                var key2 = keybind.Value.Item2;

                if (key1 != null)
                {
                    if (key1.KeyType == KeyType.Keyboard)
                    {
                        if (key1.KeyboardKey != null)
                        {
                            if (Keyboard.GetState().IsKeyDown(key1.KeyboardKey.Value))
                                yield return keybind.Key;
                        }
                    }

                    if (key1.KeyType == KeyType.Mouse)
                    {
                        if (key1.MouseKey != null)
                        {
                            if (key1.MouseKey.Value == MouseKeys.MouseLeft && Mouse.GetState().LeftButton == ButtonState.Pressed)
                                yield return keybind.Key;
                            if (key1.MouseKey.Value == MouseKeys.MouseMiddle && Mouse.GetState().MiddleButton == ButtonState.Pressed)
                                yield return keybind.Key;
                            if (key1.MouseKey.Value == MouseKeys.MouseRight && Mouse.GetState().RightButton == ButtonState.Pressed)
                                yield return keybind.Key;
                        }
                    }

                    if (key1.KeyType == KeyType.Gamepad)
                    {
                        if (key1.GamepadKey != null)
                        {
                            if (GamePad.GetState(0).IsButtonDown(key1.GamepadKey.Value))
                                yield return keybind.Key;
                        }
                    }
                }

                if (key2 != null)
                {
                    if (key2.KeyType == KeyType.Keyboard)
                    {
                        if (key2.KeyboardKey != null)
                        {
                            if (Keyboard.GetState().IsKeyDown(key2.KeyboardKey.Value))
                                yield return keybind.Key;
                        }
                    }

                    if (key2.KeyType == KeyType.Mouse)
                    {
                        if (key2.MouseKey != null)
                        {
                            if (key2.MouseKey.Value == MouseKeys.MouseLeft && Mouse.GetState().LeftButton == ButtonState.Pressed)
                                yield return keybind.Key;
                            if (key2.MouseKey.Value == MouseKeys.MouseMiddle && Mouse.GetState().MiddleButton == ButtonState.Pressed)
                                yield return keybind.Key;
                            if (key2.MouseKey.Value == MouseKeys.MouseRight && Mouse.GetState().RightButton == ButtonState.Pressed)
                                yield return keybind.Key;
                        }
                    }

                    if (key2.KeyType == KeyType.Gamepad)
                    {
                        if (key2.GamepadKey != null)
                        {
                            if (GamePad.GetState(0).IsButtonDown(key2.GamepadKey.Value))
                                yield return keybind.Key;
                        }
                    }
                }
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

        public Dictionary<ControlButtons, Tuple<GenericKey, GenericKey>> Keybinds { get; set; } = new Dictionary<ControlButtons, Tuple<GenericKey, GenericKey>>();
    }
}
