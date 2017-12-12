using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.util
{
    public static class ColorTextureFactory
    {
        private static GraphicsDevice _graphicsDevice;
        private static Dictionary<Color, Texture2D> _cache;

        public static void Initialize(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _cache = new Dictionary<Color, Texture2D>();
        }

        public static Texture2D Get(Color color)
        {
            if (!_cache.ContainsKey(color))
            {
                var texture = new Texture2D(_graphicsDevice, 1, 1);
                texture.SetData(new[] {color});
                _cache[color] = texture;
            }

            return _cache[color];
        }
    }
}