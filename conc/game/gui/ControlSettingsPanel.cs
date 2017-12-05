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

            AddRow(new KeybindRow(colorManager, font, "Move left"));
            AddRow(new KeybindRow(colorManager, font, "Move right"));
            AddRow(new KeybindRow(colorManager, font, "Jump"));
            AddRow(new KeybindRow(colorManager, font, "Fire"));
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}