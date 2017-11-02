using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.util
{
    public interface ISpriteBank
    {
        Texture2D Get(string spriteName);
    }

    public class SpriteBank : ISpriteBank
    {
        private readonly ContentManager _contentManager;
        private readonly IDictionary<string, Texture2D> _cache;

        public SpriteBank(ContentManager contentManagerManager)
        {
            _contentManager = contentManagerManager;
            _cache = new Dictionary<string, Texture2D>();
        }

        public Texture2D Get(string spriteName)
        {
            if (!_cache.TryGetValue(spriteName, out var sprite))
            {
                sprite = _contentManager.Load<Texture2D>(spriteName);
                _cache.Add(spriteName, sprite);
            }

            return sprite;
        }
    }
}