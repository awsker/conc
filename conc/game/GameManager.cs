using System.Collections.Generic;
using conc.game.scenes;
using conc.game.scenes.@base;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tile;

namespace conc.game
{
    public interface IGameManager
    {
        GraphicsDevice GraphicsDevice { get; }
    }

    public class GameManager : DrawableGameComponent, IGameManager
    {
        private readonly Game _game;
        private readonly ContentManager _contentManager;
        private SpriteBatch _spriteBatch;
        private ISpriteBank _spriteBank;

        private IGameScene _scene;

        private SpriteFont _debugFont;

        private IList<ILevel> _levels;
        
        public GameManager(Game game) : base(game)
        {
            _game = game;
            _contentManager = new ContentManager(game.Services, "content");
        }

        protected override void LoadContent()
        {
            _spriteBank = new SpriteBank(_contentManager);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _levels = LevelSerializer.DeSerialize();
            _debugFont = _contentManager.Load<SpriteFont>("fonts/debug");

            _scene = new GameScene(GraphicsDevice, _spriteBank);
            _scene.LoadContent(_contentManager);
            _scene.SetLevel(_levels[0]);
        }

        public override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _game.Exit();

            _scene.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _scene.Draw(_spriteBatch);

            for (var i = 0; i < GameDebug.Messages.Count; i++)
            {
                var message = GameDebug.Messages[i];
                _spriteBatch.DrawString(_debugFont, message, new Vector2(10, 10 + i * 16), Color.White);
            }
            GameDebug.Messages.Clear();

            _spriteBatch.End();
        }
    }
}