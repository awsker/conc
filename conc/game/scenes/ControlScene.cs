using System;
using conc.game.commands;
using conc.game.gui;
using conc.game.input;
using conc.game.scenes.baseclass;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace conc.game.scenes
{
    public interface IControlScene : IScene
    {
    }

    public class ControlScene : Scene, IControlScene
    {
        private readonly IMenu _menu;
        private ContentManager _contentManager;
        private InputManager _inputManager;

        private IControlSettingsPanel _controlSettingsPanel;

        public ControlScene(GameManager gameManager) : base(gameManager)
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

            var colorManager = _gameManager.Get<ColorManager>();
            var font = _contentManager.Load<SpriteFont>("fonts/menu");

            _controlSettingsPanel = new ControlSettingsPanel(colorManager, _inputManager, font);
            _controlSettingsPanel.ExecuteCommand += ExecuteCommand;
            GuiComponents.Add(_controlSettingsPanel);
        }

        public override void Update(GameTime gameTime)
        {
            if (HasFocus && _inputManager.IsPressed(Keys.Escape))
                ExecuteCommand(new Command(SceneType.Settings));

            foreach (var guiComponent in GuiComponents)
                guiComponent.Update(gameTime);

            _menu.Update(_inputManager);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            _menu.Draw(spriteBatch);
            foreach (var guiComponent in GuiComponents)
                guiComponent.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override bool HasFocus => !_controlSettingsPanel.HasFocus;
    }
}