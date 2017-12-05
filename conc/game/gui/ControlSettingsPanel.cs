using conc.game.extensions;
using conc.game.input;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            Position = new Vector2(500, 450);
            Size = new Vector2(600, 400);

            foreach (var keybindings in inputManger.GetKeybinds())
            {
                AddRow(new KeybindRow(colorManager, font, keybindings.Key, keybindings.Value));
            }
        }

        public override void Update(GameTime gameTime)
        {
            var mousePosition = _inputManager.GetMousePosition();

            foreach (var row in _rows)
            {
                if (row.Bounds.Intersects(mousePosition))
                {
                    row.SetHighlight();
                    continue;
                }

                row.UnsetHighlight();
            }

            base.Update(gameTime);
        }
    }
}