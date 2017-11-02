using System.Collections.Generic;
using conc.game.scenes;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tile;
using conc.game.input;
using System;
using conc.game.scenes.@base;

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

        private IScene _scene;
        private IScene _nextScene;

        private SpriteFont _debugFont;

        private IList<ILevel> _levels;
        
        public GameManager(Game game) : base(game)
        {
            _game = game;
            _contentManager = new ContentManager(game.Services, "content");
            _inputManager = new InputManager(numPlayers: 1);
        }

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
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _game.Exit();
                return;
            }
            changeSceneIfRequested();

            _scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _scene.Draw(_spriteBatch);

            _spriteBatch.Begin();

            for (var i = 0; i < GameDebug.Messages.Count; i++)
            {
                var message = GameDebug.Messages[i];
                _spriteBatch.DrawString(_debugFont, message, new Vector2(10, 10 + i * 16), Color.White);
            }
            GameDebug.Messages.Clear();

            _spriteBatch.End();            
        }

        public T Get<T>()
        {
            var t = typeof(T);
            if (_inputManager is T)
                return (T)Convert.ChangeType(_inputManager, typeof(T));
            else if(_contentManager is T)
                return (T)Convert.ChangeType(_contentManager, typeof(T));
            throw new Exception("No manager found");
        }

        public void SetScene(IScene scene)
        {
            _nextScene = scene;
            _nextScene.SetGameManager(this);
        }

        private void changeSceneIfRequested()
        {
            if (_nextScene != null)
            {
                _scene = _nextScene;
                _nextScene = null;
            }
        }
    }
}