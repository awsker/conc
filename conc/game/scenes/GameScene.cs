using System.IO;
using conc.game.scenes.@base;
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
        private ICamera _camera;
        private ContentManager _contentManager;
        private IGameManager _gameManager;

        public GameScene() : base()
        {
        }

        public override void SetGameManager(IGameManager gameManager)
        {
            _gameManager = gameManager;
            _contentManager = gameManager.Get<ContentManager>();
        }

        public override void Update(GameTime gameTime)
        {
            _camera.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _camera.Transform);

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
            _tileset = _contentManager.Load<Texture2D>(tilesetPath + _level.Tileset);

            _camera = new Camera(_level);
        }
    }
}