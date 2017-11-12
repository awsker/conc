using Microsoft.Xna.Framework;

namespace conc.game.util
{
    public static class Vector2Extension
    {
        public static Vector2 Normalized(this Vector2 v)
        {
            return Vector2.Normalize(v);
        }
    }
}
