using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.extensions
{
    public static class SpriteBatchExtension
    {
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 begin, Vector2 end, Color color, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y) angle = MathHelper.TwoPi - angle;
            var texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            spriteBatch.Draw(texture, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
