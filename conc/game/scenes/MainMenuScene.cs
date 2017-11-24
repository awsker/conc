using System;
using conc.game.commands;
using conc.game.input;
using conc.game.scenes.baseclass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.scenes
{
    public interface IMainMenuScene : IScene
    {
    }

    public class MainMenuScene : Scene, IMainMenuScene
    {
        private readonly IMenu _menu;
        private ContentManager _contentManager;
        private InputManager _inputManager;

        public MainMenuScene(GameManager gameManager) : base(gameManager)
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
                new Tuple<string, Command>("Play", new Command(SceneType.Game)),
                new Tuple<string, Command>("Settings", new Command(SceneType.Settings)),
                new Tuple<string, Command>("", new Command()),
                new Tuple<string, Command>("Quit", new Command(CommandType.Quit))
            });
        }

        public override void Update(GameTime gameTime)
        {
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