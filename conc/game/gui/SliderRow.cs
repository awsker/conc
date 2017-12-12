using conc.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui
{
    public interface ISliderRow : ISettingsPanelRow
    {
    }

    public class SliderRow : SettingsPanelRow, ISliderRow
    {
        public SliderRow(InputManager inputManager, SpriteFont font, string title) : base(inputManager, font, title)
        {
        }

        public override Rectangle FocusBounds { get; }
    }
}