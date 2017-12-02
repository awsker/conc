using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.entity.baseclass
{
    public class ImageEntity : Entity
    {
        private int _width, _height;
        private Point _offset;
        private Vector2 _drawOffset;

        public override int Width => _width;
        public override int Height => _height;
        public override Point Offset => _offset;

        private bool _loadDimensionsFromTexture;

        private Texture2D _texture;
        private string _resource;
        private Rectangle? _sourceRect;

        public ImageEntity(string resource, Rectangle? sourceRect = null)
        {
            _resource = resource;
            _sourceRect = sourceRect;
            _loadDimensionsFromTexture = true;
        }

        public ImageEntity(int width, int height, Point offset, Vector2 drawOffset, string resource, Rectangle? sourceRect = null)
        {
            _width = width;
            _height = height;
            _offset = offset;
            _drawOffset = drawOffset;
            _resource = resource;
            _sourceRect = sourceRect;
        }

        public override void LoadContent(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>(_resource);
            if (_loadDimensionsFromTexture)
            {
                _width = _sourceRect?.Width ?? _texture.Width;
                _height = _sourceRect?.Height ?? _texture.Height;
                _offset = new Point(_width / 2, _height / 2);
                _drawOffset = new Vector2(_width / 2f, _height / 2f);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(_texture, Position - new Vector2(_drawOffset.X, _drawOffset.Y), _sourceRect, Color.White);
        }
    }
}
