using System.IO;
using conc.game.scenes.@base;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tile;

namespace conc.game.scenes
{
    public interface IGameScene : IScene
    {
        void SetLevel(ILevel level);
    }

    public class GameScene : Scene, IGameScene
    {
        private ILevel _level;
        private Texture2D _tileset;
        private readonly ISpriteBank _spriteBank;
        private ICamera _camera;

        public GameScene(GraphicsDevice graphicsDevice, ISpriteBank spriteBank) : base(graphicsDevice)
        {
            _spriteBank = spriteBank;
        }

        public override void LoadContent(ContentManager contentManager)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _camera.Transform);
            //spriteBatch.Begin();

            for (var y = 0; y < _level.Width; y++)
            {
                for (var x = 0; x < _level.Height; x++)
                {
                    if (x < 0 || y < 0 || x >= _level.Width || y >= _level.Height)
                        continue;

                    var tile = _level.Tiles[x, y];
                    if (tile != null)
                        spriteBatch.Draw(_tileset, new Vector2(tile.X * GameSettings.TileSize, tile.Y * GameSettings.TileSize), tile.Source, Color.White);
                }
            }

            spriteBatch.End();
        }

        public void SetLevel(ILevel level)
        {
            _level = level;
            
            var tilesetPath = Path.GetFullPath(@"..\..\..\..\content\tiledgenerator\content\");
            _tileset = _spriteBank.Get(tilesetPath + _level.Tileset);

            _camera = new Camera(_level);
        }
    }
}