using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using conc.game.extensions;
using Microsoft.Xna.Framework;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;
using Keyboard = Microsoft.Xna.Framework.Input.Keyboard;
using Mouse = Microsoft.Xna.Framework.Input.Mouse;

namespace conc.game.input
{
    public class InputManager
    {
        private string _fileName = "config.cfg";

        private PlayerInput[] _players;
        private IDictionary<Keys, uint> _keyboardKeys;
        private IDictionary<ButtonState, uint> _mouseKeys;

        private KeyboardInput _keyboardInput;

        public InputManager(int numPlayers)
        {
            initKeyboardKeys();
            initMouseButtons();
            initPlayerInput(numPlayers);
        }

        public PlayerInput Input(int playerIndex)
        {
            return _players[playerIndex];
        }

        public bool IsDown(ControlButtons button, int playerIndex)
        {
            return _players[playerIndex].IsDown(button);
        }

        public bool IsPressed(ControlButtons button, int playerIndex)
        {
            return _players[playerIndex].IsPressed(button);
        }

        public bool IsAnyButtonPressed(int playerIndex)
        {
            foreach (var button in Enum.GetValues(typeof(ControlButtons)))
            {
                if (_players[playerIndex].IsPressed((ControlButtons) button))
                    return true;
            }

            return false;
        }

        public Keys GetNextKeyPress()
        {
            return Keyboard.GetState().GetPressedKeys().FirstOrDefault();
        }

        public bool IsUp(ControlButtons button, int playerIndex)
        {
            return _players[playerIndex].IsUp(button);
        }

        public void Update()
        {
            foreach(var p in _players)
                p.Update();
            updateKeyboardPress();
            updateMousePress();
        }

        public bool IsDown(Keys key)
        {
            return _keyboardKeys[key] > 0;
        }

        public bool IsPressed(Keys key)
        {
            return _keyboardKeys[key] == 1;
        }

        public bool IsUp(Keys key)
        {
            return _keyboardKeys[key] == 0;
        }

        public bool IsDown(ButtonState button)
        {
            return _mouseKeys[button] > 0;
        }

        public bool IsPressed(ButtonState button)
        {
            return _mouseKeys[button] == 1;
        }

        public bool IsUp(ButtonState button)
        {
            return _mouseKeys[button] == 0;
        }

        public bool IsMouseDownOverBounds(Rectangle bounds, int playerIndex)
        {
            var mouseState = Mouse.GetState();
            return bounds.Intersects(mouseState.Position) && IsPressed(ControlButtons.FireRope, playerIndex);
        }

        public Point GetMousePosition()
        {
            return Mouse.GetState().Position;
        }

        private void initPlayerInput(int numPlayers)
        {
            _players = new PlayerInput[numPlayers];
            for (int i = 0; i < numPlayers; ++i)
                _players[i] = new PlayerInput(i);

            _keyboardInput = new KeyboardInput();
            _keyboardInput.Keybinds = GetKeybinds();

            _players[0].InputDevices.Add(_keyboardInput);
            _players[0].InputDevices.Add(new MouseInput());
        }

        private void initKeyboardKeys()
        {
            _keyboardKeys = new Dictionary<Keys, uint>();
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                _keyboardKeys[key] = 0;
            }
        }

        private void initMouseButtons()
        {
            _mouseKeys = new Dictionary<ButtonState, uint>();
            foreach (ButtonState button in Enum.GetValues(typeof(ButtonState)))
            {
                _mouseKeys[button] = 0;
            }
        }

        private void updateKeyboardPress()
        {
            foreach(Keys key in Enum.GetValues(typeof(Keys)))
            {
                if(Keyboard.GetState().IsKeyDown(key))
                {
                    ++_keyboardKeys[key];
                }
                else
                {
                    _keyboardKeys[key] = 0;
                }
            }
        }

        private void updateMousePress()
        {
            foreach (ButtonState button in Enum.GetValues(typeof(ButtonState)))
            {
                if (Mouse.GetState().LeftButton == button)
                {
                    ++_mouseKeys[button];
                }
                else
                {
                    _mouseKeys[button] = 0;
                }
            }
        }

        public void WriteDefaultConfig()
        {
            if (File.Exists(_fileName))
                return;

            using (var file = new StreamWriter(_fileName))
            {
                file.WriteLine(ControlButtons.MoveUp + "=" + Keys.W);
                file.WriteLine(ControlButtons.MoveDown + "=" + Keys.A);
                file.WriteLine(ControlButtons.MoveLeft + "=" + Keys.S);
                file.WriteLine(ControlButtons.MoveRight + "=" + Keys.D);
                file.WriteLine(ControlButtons.FireRope + "=" + Keys.Space);
                file.WriteLine(ControlButtons.Jump + "=" + Keys.LeftShift);
            }

            _keyboardInput.Keybinds = GetKeybinds();
        }

        public void WriteConfigWithNewKey(ControlButtons button, Keys key)
        {
            if (!File.Exists(_fileName))
                WriteDefaultConfig();

            var config = File.ReadAllLines(_fileName);

            for (var i = 0; i < config.Length; i++)
            {
                var line = config[i];
                if (line.StartsWith(button.ToString()))
                    config[i] = button + "=" + key;
            }

            File.WriteAllLines(_fileName, config);

            _keyboardInput.Keybinds = GetKeybinds();
        }

        public Dictionary<ControlButtons, Keys> GetKeybinds()
        {
            var ret = new Dictionary<ControlButtons, Keys>();

            if (!File.Exists(_fileName))
                return ret;

            var config = File.ReadAllLines(_fileName);

            foreach (var line in config)
            {
                var controlButton = (ControlButtons)Enum.Parse(typeof(ControlButtons), line.Trim().Split('=')[0]);
                var key = (Keys)Enum.Parse(typeof(Keys), line.Split('=')[1], true);

                ret.Add(controlButton, key);
            }

            return ret;
        }
    }

    public enum ControlButtons
    {
        MoveLeft, MoveRight, MoveUp, MoveDown, Jump, FireRope
    }
}
