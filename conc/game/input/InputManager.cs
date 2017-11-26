using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace conc.game.input
{
    public class InputManager
    {
        private PlayerInput[] _players;
        private IDictionary<Keys, uint> _keyboardKeys;
        //private IDictionary<ButtonState, uint> _mouseKeys;

        public InputManager(int numPlayers)
        {
            initKeyboardKeys();
            initPlayerInput(numPlayers);
        }

        public void LoadInputConfig(string file, int playerIndex)
        {

        }

        public void SaveInputConfig(string file, int playerIndex)
        {

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

        public bool IsUp(ControlButtons button, int playerIndex)
        {
            return _players[playerIndex].IsUp(button);
        }

        public void Update()
        {
            foreach(var p in _players)
                p.Update();
            updateKeyboardPress();
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

        public Point GetMousePosition()
        {
            return Mouse.GetState().Position;
        }

        private void initPlayerInput(int numPlayers)
        {
            _players = new PlayerInput[numPlayers];
            for (int i = 0; i < numPlayers; ++i)
                _players[i] = new PlayerInput(i);

            _players[0].InputDevices.Add(new KeyboardInput());
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
    }

    public enum ControlButtons
    {
        Left, Right, Up, Down, Jump, FireRope
    }
}
