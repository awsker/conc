using conc.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui
{
    public interface IControlSettingsPanel : ISettingsPanel
    {
    }

    public class ControlSettingsPanel : SettingsPanel, IControlSettingsPanel
    {
        private KeybindRow _currentRow;

        public ControlSettingsPanel(InputManager inputManager, SpriteFont font) : base(inputManager, font)
        {
            Position = new Vector2(400, 450);
            Size = new Vector2(800, 400);

            foreach (var keybindings in inputManager.GetKeybinds())
            {
                AddRow(new KeybindRow(inputManager, font, keybindings.Key, keybindings.Value.Item1, keybindings.Value.Item2));
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var row in _rows)
            {
                if (row is KeybindRow keybindRow)
                {
                    if (_inputManager.IsMouseDownOverBounds(keybindRow.Key1Bounds) || _inputManager.IsMouseDownOverBounds(keybindRow.Key2Bounds))
                    {
                        if (_currentRow != null && _currentRow != keybindRow)
                        {
                            _currentRow.Key1Activated = false;
                            _currentRow.Key2Activated = false;
                        }

                        _currentRow = keybindRow;
                    }
                }
                
            }

            base.Update(gameTime);
        }
    }
}