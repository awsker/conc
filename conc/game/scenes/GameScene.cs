using System.IO;
using System.Linq;
using System.Xml;
using conc.game.entity;
using conc.game.entity.animation;
using conc.game.input;
using conc.game.scenes.baseclass;
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
        private Texture2D[] _tilesetTextures;
        private ICamera _camera;
        private ContentManager _contentManager;
        private IGameManager _gameManager;
        private InputManager _inputManager;
        private VideoModeManager _videoManager;
        private IPlayer _player;

        public override void SetGameManager(IGameManager gameManager)
        {
            _gameManager = gameManager;
            _contentManager = gameManager.Get<ContentManager>();
            _inputManager = gameManager.Get<InputManager>();
            _videoManager = gameManager.Get<VideoModeManager>();
        }

        public override void Update(GameTime gameTime)
        {
            var playerPosition = _player.Transform.Position;

            if (_inputManager.IsDown(ControlButtons.Right, 0))
                playerPosition.X += (float) gameTime.ElapsedGameTime.TotalSeconds * 150f;
            if (_inputManager.IsDown(ControlButtons.Left, 0))
                playerPosition.X -= (float) gameTime.ElapsedGameTime.TotalSeconds * 150f;
            if (_inputManager.IsDown(ControlButtons.Up, 0))
                playerPosition.Y -= (float) gameTime.ElapsedGameTime.TotalSeconds * 150f;
            if (_inputManager.IsDown(ControlButtons.Down, 0))
                playerPosition.Y += (float) gameTime.ElapsedGameTime.TotalSeconds * 150f;

            _player.Transform.Position = playerPosition;

            _camera.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null,
                _camera.Transform);

            drawLevel(spriteBatch);

            foreach (var entity in Entities.Where(e => e.IsVisible))
                entity.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void drawLevel(SpriteBatch spriteBatch)
        {
            for (var y = 0; y < _level.Width; y++)
            {
                for (var x = 0; x < _level.Height; x++)
                {
                    if (x < 0 || y < 0 || x >= _level.Width || y >= _level.Height)
                        continue;

                    var tile = _level.Tiles[x, y];

                    if (tile != null)
                    {
                        var tileset = _level.Tilesets[tile.TilesetIndex];
                        spriteBatch.Draw(_tilesetTextures[tile.TilesetIndex], new Vector2(tile.X * tileset.TileWidth, tile.Y * tileset.TileHeight), tile.Source, Color.White);
                    }
                }
            }
        }

        public void SetLevel(ILevel level)
        {
            Entities.Clear();
            _level = level;
            
            var tilesetPath = Path.GetFullPath(@"..\..\..\..\content\tiledgenerator\content\");
            _tilesetTextures = new Texture2D[_level.Tilesets.Length];
            for (int i = 0; i < _level.Tilesets.Length; ++i)
            {
                var tileset = _level.Tilesets[i];
                _tilesetTextures[i] = _contentManager.Load<Texture2D>(tilesetPath + tileset.Source);
            }
            _player = new Player(_level.Start);
            _player.LoadContent(_contentManager);
            Entities.Add(_player);

            _camera = new Camera(_videoManager.GraphicsDeviceManager);
            _camera.SetTarget(_player);

            var animationReader = new AnimationReader();
            animationReader.LoadAllTemplates();

            var animationGroup = animationReader.GetAnimationGroup("Player");
        }
    }
}