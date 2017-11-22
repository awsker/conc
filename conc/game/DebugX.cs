using conc.game.entity.baseclass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game
{
    public class DebugX : Entity
    {
        private readonly Texture2D _texture;
        private readonly Vector2 _offset;
        public DebugX(Vector2 position, Texture2D img)
        {
            Position = position;
            _texture = img;
            _offset = new Vector2(img.Width / 2f, img.Height /2f);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Transform.Position - _offset, Color.White);
        }
    }
}
