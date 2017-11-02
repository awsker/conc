using System;

namespace conc.game.input
{
    public class ButtonsContainer
    {
        private uint[] _framesHeld;

        public ButtonsContainer()
        {
            _framesHeld = new uint[Enum.GetValues(typeof(ControlButtons)).Length];
        }

        public void Increment(ControlButtons button)
        {
            ++_framesHeld[(int)button];
        }

        public bool IsDown(ControlButtons button)
        {
            return _framesHeld[(int)button] > 0;
        }

        public bool IsPressed(ControlButtons button)
        {
            return _framesHeld[(int)button] == 1;
        }

        public bool IsUp(ControlButtons button)
        {
            return _framesHeld[(int)button] == 0;
        }

        public void Release(ControlButtons button)
        {
            _framesHeld[(int)button] = 0;
        }
    }
}
