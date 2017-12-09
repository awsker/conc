using System;
using conc.game.extensions;
using conc.game.input;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace conc.game.gui
{
    public interface IControlSettingsPanel : ISettingsPanel
    {
    }

    public class ControlSettingsPanel : SettingsPanel, IControlSettingsPanel
    {
        private ISettingsPanelRow _currentRow;
        private ISettingsPanelRow _highlightRow;

        public ControlSettingsPanel(ColorManager colorManager, InputManager inputManger, SpriteFont font) : base(colorManager, inputManger, font)
        {
            Position = new Vector2(400, 450);
            Size = new Vector2(800, 400);

            foreach (var keybindings in inputManger.GetKeybinds())
            {
                AddRow(new KeybindRow(colorManager, font, keybindings.Key, keybindings.Value.Item1, keybindings.Value.Item2));
            }
        }

        public override void Update(GameTime gameTime)
        {
            var mousePosition = _inputManager.GetMousePosition();
            var nextKeyPress = _inputManager.GetNextKeyPress();

            foreach (var row in _rows)
            {
                if (row is IKeybindRow keybindRow)
                {
                    keybindRow.UnsetKeyHighlights();

                    if (keybindRow.Key1Bounds.Intersects(mousePosition))
                        keybindRow.SetKey1Highlight();

                    if (keybindRow.Key2Bounds.Intersects(mousePosition))
                        keybindRow.SetKey2Highlight();
                }
            }

            base.Update(gameTime);
        }
    }
}