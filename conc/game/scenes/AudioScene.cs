using System;
using System.Collections.Generic;
using conc.game.commands;
using conc.game.gui;
using conc.game.gui.baseclass;
using conc.game.input;
using conc.game.scenes.baseclass;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace conc.game.scenes
{
    public interface IAudioScene : IScene
    {
    }

    public class AudioScene : Scene, IAudioScene
    {
        private readonly IMenu _menu;
        private ContentManager _contentManager;
        private InputManager _inputManager;
        
        public AudioScene(GameManager gameManager) : base(gameManager)
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

            var panel = new Panel(colorManager)
            {
                Position = new Vector2(200f, 200f),
                Size = new Vector2(400f, 600f),
                BackgroundColor = new Color(105, 143, 224)
            };

            var subPanel = new Panel(colorManager)
            {
                Position = new Vector2(0f, 0f),
                Size = new Vector2(400f, 100f),
                BackgroundColor = new Color(255, 255, 255)
            };

            var label = new Label(colorManager, _contentManager.Load<SpriteFont>("fonts/menu"))
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Margin(20f, 0f, 0f, 0f),
                Text = "Controls",
                ForegroundColor = new Color(53, 81, 141)
            };

            subPanel.AddChild(label);
            panel.AddChild(subPanel);

            GuiComponents.Add(panel);
        }

        public override void Update(GameTime gameTime)
        {
            GameDebug.Log("Mouse", _inputManager.GetMousePosition());

            if (_inputManager.IsPressed(Keys.Escape))
                ExecuteCommand(new Command(SceneType.Settings));

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
    }
}