using conc.game;
using Microsoft.Xna.Framework;

namespace conc
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        public Main()
        {
            var gameManager = new GameManager(this);
            Components.Add(gameManager);
        }
    }
}
