using System;
using System.Collections.Generic;

namespace conc.game.input
{
    public class PlayerInput
    {
        private int _playerIndex;
        private ButtonsContainer _buttons;
        private IList<IInputDevice> _inputDevices;

        public PlayerInput(int playerIndex)
        {
            _playerIndex = playerIndex;
            _buttons = new ButtonsContainer();
            _inputDevices = new List<IInputDevice>();
        }

        public void Update()
        {
            var buttonsEnumValues = Enum.GetValues(typeof(ControlButtons));
            bool[] buttonsDown = new bool[buttonsEnumValues.Length];
            
            foreach (var input in _inputDevices)
            {
                foreach(var button in input.GetButtonsDown()) //Get all inputs from all the input devices. Note any buttons that are pressed
                {
                    buttonsDown[(int)button] = true;
                }
            }
            foreach(ControlButtons button in buttonsEnumValues)
            {
                if (buttonsDown[(int)button]) //Is button pressed?
                    _buttons.Increment(button);
                else
                    _buttons.Release(button);
            }
        }

        public bool IsDown(ControlButtons button)
        {
            return _buttons.IsDown(button);
        }

        public bool IsPressed(ControlButtons button)
        {
            return _buttons.IsPressed(button);
        }

        public bool IsUp(ControlButtons button)
        {
            return _buttons.IsUp(button);
        }

        public IList<IInputDevice> InputDevices => _inputDevices;
    }
}
