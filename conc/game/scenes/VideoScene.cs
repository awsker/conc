using System;
using conc.game.commands;
using conc.game.input;
using conc.game.scenes.baseclass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace conc.game.scenes
{
    public interface IVideoScene : IScene
    {
    }

    public class VideoScene : Scene, IVideoScene
    {
        private readonly IMenu _menu;
        private ContentManager _contentManager;
        private InputManager _inputManager;

        public VideoScene(GameManager gameManager) : base(gameManager)
        {
            _menu = new Menu(new Vector2(GameSettings.TargetWidth - 50f, GameSettings.TargetHeight - 50f));
            _menu.ExecuteCommand += ExecuteCommand;
        }

        public override void LoadContent()
        {
            _contentManager = _gameManager.Get<ContentManager>();
            _inputManager = _gameManager.Get<InputManager>();

            _menu.LoadContent(_contentManager, new[]
            {
                new Tuple<string, Command>("Back", new Command(SceneType.Settings))
            });
        }

        public override void Update(GameTime gameTime)
        {
            if (_inputManager.IsPressed(Keys.Escape))
                ExecuteCommand(new Command(SceneType.Settings));

            _menu.Update(_inputManager);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _menu.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}