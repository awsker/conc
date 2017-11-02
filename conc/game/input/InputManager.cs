namespace conc.game.input
{
    public class InputManager
    {
        private PlayerInput[] _players;

        public InputManager(int numPlayers)
        {
            _players = new PlayerInput[numPlayers];
            for(int i = 0; i < numPlayers; ++i)
                _players[i] = new PlayerInput(i);

            _players[0].InputDevices.Add(new KeyboardInput());
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

        public bool IsUp(ControlButtons button, int playerIndex)
        {
            return _players[playerIndex].IsUp(button);
        }

        public void Update()
        {
            foreach(var p in _players)
                p.Update();
        }
    }

    public enum ControlButtons
    {
        Left, Right, Up, Down, Jump
    }
}
