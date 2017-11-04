using System.Collections.Generic;
using System.IO;
using conc.game.entity;
using conc.game.entity.@base;
using conc.game.input;
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
        private ICamera _camera;
        private ContentManager _contentManager;
        private IGameManager _gameManager;
        private InputManager _inputManager;
        private VideoModeManager _videoManager;
        private IPlayer _player;

        private readonly IList<IEntity> _entities = new List<IEntity>();

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
                playerPosition.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * 150f;
            if (_inputManager.IsDown(ControlButtons.Up, 0))
                playerPosition.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 150f;
            if (_inputManager.IsDown(ControlButtons.Down, 0))
                playerPosition.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 150f;

            _player.Transform.Position = playerPosition;

            _camera.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.LinearClamp, null, null, null, _camera.Transform);

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

            foreach (var entity in _entities)
                entity.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void SetLevel(ILevel level)
        {
            _entities.Clear();
            _level = level;
            
            var tilesetPath = Path.GetFullPath(@"..\..\..\..\content\tiledgenerator\content\");
            _tileset = _contentManager.Load<Texture2D>(tilesetPath + _level.Tileset);

            _player = new Player(_level.Start);
            _player.LoadContent(_contentManager);
            _entities.Add(_player);

            _camera = new Camera(_videoManager.GraphicsDeviceManager);
            _camera.SetTarget(_player);
        }
    }
}