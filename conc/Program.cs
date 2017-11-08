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
            /*
            using (var game = new Main())
                game.Run();
            */
            var line1 = new Line(Vector2.Zero, new Vector2(-2f, -2f), LineType.InfiniteAtTarget);
            var line2 = new Line(new Vector2(4f, 0f), new Vector2(4f, 2f), LineType.BothCapped);

            Vector2 closest;
            for (int i = 0; i < 100000; ++i)
            {
                line1.Intersecting(line2);
            }
            var time = Line.Stop.ElapsedMilliseconds;
        }
    }
}
