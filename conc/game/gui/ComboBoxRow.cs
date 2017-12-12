using conc.game.input;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui
{
    public interface IComboBoxRow : ISettingsPanelRow
    {
    }

    public class ComboBoxRow : SettingsPanelRow, IComboBoxRow
    {
        public ComboBoxRow(InputManager inputManager, SpriteFont font, string title) : base(inputManager, font, title)
        {
        }

        public override Rectangle FocusBounds { get; }
    }
}