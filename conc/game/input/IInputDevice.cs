using System.Collections.Generic;

namespace conc.game.input
{
    public interface IInputDevice
    {
        IEnumerable<ControlButtons> GetButtonsDown();
    }
}
