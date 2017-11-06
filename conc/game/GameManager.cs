using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using conc.game.scenes.baseclass;
using conc.game.scenes;
using conc.game.util;
using conc.game.input;
using tile;
using System;

namespace conc.game
{
    public interface IGameManager
    {
        GraphicsDevice GraphicsDevice { get; }
        T Get<T>();
        void SetScene(IScene scene);
    }

    public class GameManager : DrawableGameComponent, IGameManager
    {
        private readonly Game _game;
        private readonly ContentManager _contentManager;
        private SpriteBatch _spriteBatch;
        private InputManager _inputManager;
        private VideoModeManager _videoManager;

        private IScene _scene;
        private IScene _nextScene;

        private SpriteFont _debugFont;

        private IList<ILevel> _levels;

        #region Constructor
        public GameManager(Game game) : base(game)
        {
            _game = game;
            _game.IsFixedTimeStep = false;
            _contentManager = new ContentManager(game.Services, "content");
            _inputManager = new InputManager(numPlayers: 1);
            initVideoManager();
        }
        #endregion

        #region Private methods
        private void changeSceneIfRequested()
        {
            if (_nextScene != null)
            {
                _scene = _nextScene;
                _nextScene = null;
            }
        }

        private void initVideoManager()
        {
            var deviceManager = new GraphicsDeviceManager(_game)
            {
                PreferredBackBufferWidth = GameSettings.PreferredBackBufferWidth,
                PreferredBackBufferHeight = GameSettings.PreferredBackBufferHeight,
                SynchronizeWithVerticalRetrace = true,
                PreferMultiSampling = false
            };
            _videoManager = new VideoModeManager(deviceManager, _game.Window);
        }
        #endregion

        #region IGameManager implementation
        /// <summary>
        /// Gets a manager of type T
        /// </summary>
        /// <typeparam name="T">Type of manager to fetch</typeparam>
        /// <returns></returns>
        public T Get<T>()
        {
            if (_inputManager is T)
                return (T)Convert.ChangeType(_inputManager, typeof(T));
            else if (_contentManager is T)
                return (T)Convert.ChangeType(_contentManager, typeof(T));
            else if (_videoManager is T)
                return (T)Convert.ChangeType(_videoManager, typeof(T));

            throw new Exception("No manager found");
        }

        /// <summary>
        /// Sets the scene to change to before the next update
        /// </summary>
        /// <param name="scene"></param>
        public void SetScene(IScene scene)
        {
            _nextScene = scene;
            _nextScene.SetGameManager(this);
        }
        #endregion

        #region GameComponent Overrides 
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _levels = LevelSerializer.DeSerialize();
            _debugFont = _contentManager.Load<SpriteFont>("fonts/debug");

            var gamescene = new GameScene();
            SetScene(gamescene);

            gamescene.SetLevel(_levels[0]);
        }
        
        public override void Update(GameTime gameTime)
        {
            _inputManager.Update();
            if (_inputManager.IsPressed(Keys.Escape))
            {
                _game.Exit();
                return;
            }
            
            if (_inputManager.IsPressed(Keys.F11))
                _videoManager.ToggleFullscreen();

            changeSceneIfRequested();

            _scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _scene.Draw(_spriteBatch);

            _spriteBatch.Begin();

            GameDebug.Messages.Add("Frame rate: " + 1 / gameTime.ElapsedGameTime.TotalSeconds);
            for (var i = 0; i < GameDebug.Messages.Count; i++)
            {
                var message = GameDebug.Messages[i];
                _spriteBatch.DrawString(_debugFont, message, new Vector2(10, 10 + i * 16), Color.White);
            }
            GameDebug.Messages.Clear();

            _spriteBatch.End();
        }
        #endregion

    }
}