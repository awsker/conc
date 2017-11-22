using System.Linq;
using conc.game.entity;
using conc.game.entity.animation;
using conc.game.entity.movement;
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
        ILevel CurrentLevel { get; }
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
        private IAnimationReader _animationReader;

        public override IGameManager GameManager
        {
            get { return _gameManager; }
            set
            {
                _gameManager = value;
                _contentManager = value.Get<ContentManager>();
                _inputManager = value.Get<InputManager>();
                _videoManager = value.Get<VideoModeManager>();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < _entities.Count; ++i)
            {
                _entities[i].Update(gameTime);
            }

            var movementHandler = new MovementHandler();
            for (int i = 0; i < _entities.Count; ++i)
            {
                if(_entities[i] is IMovingEntity e)
                    movementHandler.HandleMovement(gameTime, e, _level.CollisionLines);
            }

            _camera.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null,
                _camera.Transform);

            DrawLevel(spriteBatch);

            foreach (var entity in Entities.Where(e => e.IsVisible))
                entity.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void DrawLevel(SpriteBatch spriteBatch)
        {
            var playerX = (int)_player.Transform.Position.X / GameSettings.TileSize;
            var playerY = (int)_player.Transform.Position.Y / GameSettings.TileSize;

            for (var y = playerY - 25; y <= playerY + 25; y++)
            {
                for (var x = playerX - 25; x <= playerX + 25; x++)
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
            _entities.Clear();
            _level = level;

            var tilesetPath = @"tilesets\";
            _tilesetTextures = new Texture2D[_level.Tilesets.Length];
            for (int i = 0; i < _level.Tilesets.Length; ++i)
            {
                var tileset = _level.Tilesets[i];
                _tilesetTextures[i] = _contentManager.Load<Texture2D>(tilesetPath + tileset.Source);
            }

            _animationReader = new AnimationReader();
            _animationReader.LoadAllTemplates();
            
            _player = new Player(new Vector2(_level.Start.X + _level.Start.Width/2f, _level.Start.Top), _animationReader.GetAnimator("Player"));
            _player.LoadContent(_contentManager);
            AddEntity(_player);

            _camera = new Camera(_videoManager.GraphicsDeviceManager);
            _camera.SetTarget(_player);
        }

        public ILevel CurrentLevel => _level;
    }
}