using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.util
{
    public class ColorManager
    {
        private readonly GraphicsDevice _graphicsDevice;
        private readonly Dictionary<Color, Texture2D> _cache;

        public ColorManager(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
            _cache = new Dictionary<Color, Texture2D>();
        }

        public Texture2D Get(Color color)
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