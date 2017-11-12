using System.Diagnostics.Contracts;
using Microsoft.Xna.Framework;

namespace conc.game.util
{
    public static class Vector2Extension
    {
        [Pure]
        public static Vector2 Normalized(this Vector2 v)
        {
            return Vector2.Normalize(v);
        }

        [Pure]
        public static Vector2 Add(this Vector2 v, Vector2 v2)
        {
            return Vector2.Add(v, v2);
        }

        [Pure]
        public static Vector2 AddX(this Vector2 v, float x)
        {
            return new Vector2(v.X + x, v.Y);
        }

        [Pure]
        public static Vector2 AddY(this Vector2 v, float y)
        {
            return new Vector2(v.X, v.Y + y);
        }
    }
}
