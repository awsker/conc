using System.Linq;
using conc.game.commands;
using conc.game.entity;
using conc.game.entity.animation;
using conc.game.entity.movement;
using conc.game.input;
using conc.game.scenes.baseclass;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private ICamera _camera;
        private ContentManager _contentManager;
        private InputManager _inputManager;
        private VideoModeManager _videoManager;
        private IPlayer _player;
        private IAnimationReader _animationReader;

        public GameScene(GameManager gameManager) : base(gameManager)
        {
        }

        public override void LoadContent()
        {
            _contentManager = _gameManager.Get<ContentManager>();
            _inputManager = _gameManager.Get<InputManager>();
            _videoManager = _gameManager.Get<VideoModeManager>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_inputManager.IsPressed(Keys.Escape))
                ExecuteCommand(new Command(SceneType.MainMenu));

            for (int i = 0; i < _entities.Count; ++i)
            {
                _entities[i].Update(gameTime);
            }

            IMovementHandler movementHandler = new MovementHandler();
            for (int i = 0; i < _entities.Count; ++i)
            {
                if(_entities[i] is IMovingEntity e)
                    movementHandler.HandleMovement(gameTime, e, _level);
            }

            _camera.Update(gameTime);

            RemoveDestroyedEntities();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null, null, _camera.Transform);

            DrawLevel(spriteBatch);

            foreach (var entity in Entities.Where(e => e.IsVisible))
                entity.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void DrawLevel(SpriteBatch spriteBatch)
        {
            _level.DrawLevel(spriteBatch.GraphicsDevice);
        }
       
        public void SetLevel(ILevel level)
        {
            _entities.Clear();
            _level = level;

            _level.LoadContent(GameManager.GraphicsDevice, GameManager.Get<ContentManager>());

            _animationReader = new AnimationReader();
            _animationReader.LoadAllTemplates();
            
            _player = new Player(new Vector2(_level.Start.X, _level.Start.Y), _animationReader.GetAnimator("Player"));
            AddEntity(_player);

            var camera = new Camera(_videoManager.GraphicsDeviceManager);
            camera.SetTarget(_player);
            camera.SetLevel(level);
            camera.KeepCameraInsideBounds = true;
            _camera = camera;
        }

        public ILevel CurrentLevel => _level;
    }
}