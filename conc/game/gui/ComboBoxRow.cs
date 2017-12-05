using conc.game.util;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui
{
    public interface IComboBoxRow : ISettingsPanelRow
    {
    }

    public class ComboBoxRow : SettingsPanelRow, IComboBoxRow
    {
        public ComboBoxRow(ColorManager colorManager, SpriteFont font, string title) : base(colorManager, font, title)
        {
        }
    }
}