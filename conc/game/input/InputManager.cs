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
        private IDictionary<MouseKeys, uint> _mouseKeys;

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

        public Keys GetNextKeyPress()
        {
            var firstKey = Keyboard.GetState().GetPressedKeys().FirstOrDefault();
            if (IsPressed(firstKey))
                return firstKey;

            return Keys.None;
        }

        public MouseKeys GetNextMouseKeyPress()
        {
            if (IsPressed(MouseKeys.MouseLeft))
                return MouseKeys.MouseLeft;
            if (IsPressed(MouseKeys.MouseMiddle))
                return MouseKeys.MouseMiddle;
            if (IsPressed(MouseKeys.MouseRight))
                return MouseKeys.MouseRight;

            return MouseKeys.None;
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

        public bool IsDown(MouseKeys key)
        {
            return _mouseKeys[key] > 0;
        }

        public bool IsPressed(MouseKeys key)
        {
            return _mouseKeys[key] == 1;
        }

        public bool IsUp(MouseKeys key)
        {
            return _mouseKeys[key] == 0;
        }

        public bool IsMouseDownOverBounds(Rectangle bounds, int playerIndex)
        {
            var mouseState = Mouse.GetState();
            return bounds.Intersects(mouseState.Position) && IsPressed(MouseKeys.MouseLeft);
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
            //_players[0].InputDevices.Add(new MouseInput());
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
            _mouseKeys = new Dictionary<MouseKeys, uint>();
            foreach (MouseKeys key in Enum.GetValues(typeof(MouseKeys)))
                _mouseKeys[key] = 0;
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
            foreach (MouseKeys mouseKey in Enum.GetValues(typeof(MouseKeys)))
            {
                var mouseState = Mouse.GetState();
                switch (mouseKey)
                {
                    case MouseKeys.None:
                        break;
                    case MouseKeys.MouseLeft:
                        if (mouseState.LeftButton == ButtonState.Pressed) ++_mouseKeys[mouseKey];
                        else _mouseKeys[mouseKey] = 0;
                        break;
                    case MouseKeys.MouseMiddle:
                        if (mouseState.MiddleButton == ButtonState.Pressed) ++_mouseKeys[mouseKey];
                        else _mouseKeys[mouseKey] = 0;
                        break;
                    case MouseKeys.MouseRight:
                        if (mouseState.RightButton == ButtonState.Pressed) ++_mouseKeys[mouseKey];
                        else _mouseKeys[mouseKey] = 0;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
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
                file.WriteLine(ControlButtons.MoveDown + "=" + Keys.S);
                file.WriteLine(ControlButtons.MoveLeft + "=" + Keys.A);
                file.WriteLine(ControlButtons.MoveRight + "=" + Keys.D);
                file.WriteLine(ControlButtons.FireRope + "=" + Keys.LeftShift);
                file.WriteLine(ControlButtons.Jump + "=" + Keys.Space);
            }

            _keyboardInput.Keybinds = GetKeybinds();
        }

        public void WriteConfigWithNewKey(ControlButtons controlButton, string key1, string key2)
        {
            if (!File.Exists(_fileName))
                WriteDefaultConfig();

            var config = File.ReadAllLines(_fileName);

            for (var i = 0; i < config.Length; i++)
            {
                var line = config[i];
                if (line.StartsWith(controlButton.ToString()))
                    config[i] = controlButton + "=" + key1 + (key2 != string.Empty ? "," + key2 : string.Empty);
            }

            File.WriteAllLines(_fileName, config);

            _keyboardInput.Keybinds = GetKeybinds();
        }

        public Dictionary<ControlButtons, Tuple<GenericKey, GenericKey>> GetKeybinds()
        {
            var ret = new Dictionary<ControlButtons, Tuple<GenericKey, GenericKey>>();

            if (!File.Exists(_fileName))
                return ret;

            var config = File.ReadAllLines(_fileName);

            foreach (var line in config)
            {
                var controlButtonLine = line.Trim().Split('=')[0];
                var keyLine1 = line.Split('=')[1];

                var keyLine2 = string.Empty;
                if (keyLine1.Contains(','))
                {
                    keyLine2 = keyLine1.Split(',')[1].Trim();
                    keyLine1 = keyLine1.Split(',')[0].Trim();
                }

                var controlButton = (ControlButtons)Enum.Parse(typeof(ControlButtons), controlButtonLine);

                GenericKey key1 = null;
                GenericKey key2 = null;

                Keys? keyboardKey1 = null;
                if (Enum.TryParse(keyLine1, out Keys tempKeyboardKey1))
                    keyboardKey1 = tempKeyboardKey1;

                MouseKeys? mouseKey1 = null;
                if (Enum.TryParse(keyLine1, out MouseKeys tempMouseKey1))
                    mouseKey1 = tempMouseKey1;

                Buttons? gamepadKey1 = null;
                if (Enum.TryParse(keyLine1, out Buttons tempGamepadKey1))
                    gamepadKey1 = tempGamepadKey1;

                if (keyboardKey1 != null)
                    key1 = new GenericKey(keyboardKey1);
                else if (mouseKey1 != null)
                    key1 = new GenericKey(mouseKey1);
                else if (gamepadKey1 != null)
                    key1 = new GenericKey(gamepadKey1);

                if (keyLine2 != string.Empty)
                {
                    Keys? keyboardKey2 = null;
                    if (Enum.TryParse(keyLine2, out Keys tempKeyboardKey2))
                        keyboardKey2 = tempKeyboardKey2;

                    MouseKeys? mouseKey2 = null;
                    if (Enum.TryParse(keyLine2, out MouseKeys tempMouseKey2))
                        mouseKey2 = tempMouseKey2;

                    Buttons? gamepadKey2 = null;
                    if (Enum.TryParse(keyLine2, out Buttons tempGamepadKey2))
                        gamepadKey2 = tempGamepadKey2;

                    if (keyboardKey2 != null)
                        key2 = new GenericKey(keyboardKey2);
                    else if (mouseKey2 != null)
                        key2 = new GenericKey(mouseKey2);
                    else if (gamepadKey2 != null)
                        key2 = new GenericKey(gamepadKey2);
                }

                ret.Add(controlButton, Tuple.Create(key1, key2));
            }

            return ret;
        }
    }

    public enum ControlButtons
    {
        MoveLeft, MoveRight, MoveUp, MoveDown, Jump, FireRope
    }

    public enum MouseKeys
    {
        None,
        MouseLeft,
        MouseMiddle,
        MouseRight
    }

    public enum KeyType
    {
        Keyboard,
        Mouse,
        Gamepad
    }

    public class GenericKey
    {
        public GenericKey(Keys? key)
        {
            KeyboardKey = key;
            KeyType = KeyType.Keyboard;
        }

        public GenericKey(MouseKeys? key)
        {
            MouseKey = key;
            KeyType = KeyType.Mouse;
        }

        public GenericKey(Buttons? key)
        {
            GamepadKey = key;
            KeyType = KeyType.Gamepad;
        }

        public KeyType KeyType { get; }
        public Keys? KeyboardKey { get; }
        public MouseKeys? MouseKey { get; }
        public Buttons? GamepadKey { get; }

        public string Text
        {
            get
            {
                if (KeyType == KeyType.Keyboard)
                    return KeyboardKey?.ToString();
                if (KeyType == KeyType.Mouse)
                    return MouseKey?.ToString();
                return GamepadKey?.ToString();
            }       
        }
    }
}
