using System;
using conc.game.math;
using Microsoft.Xna.Framework;

namespace conc
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        { 
            using (var game = new Main())
                game.Run();
        }
    }
}
