using Microsoft.Xna.Framework;

namespace conc.game.extensions
{
    public static class RectangleExtension
    {
        public static bool Intersects(this Rectangle rect, Point point)
        {
            return point.X >= rect.X && point.X <= rect.X + rect.Width && point.Y >= rect.Y && point.Y <= rect.Y + rect.Height;
        }
    }
}