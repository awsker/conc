using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui
{
    public interface ISliderRow : ISettingsPanelRow
    {
    }

    public class SliderRow : SettingsPanelRow, ISliderRow
    {
        public SliderRow(ColorManager colorManager, SpriteFont font, string title) : base(colorManager, font, title)
        {
        }

        public override Rectangle FocusBounds { get; }
    }
}