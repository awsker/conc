using conc.game.gui.components;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui
{
    public interface ISettingsPanelRow : IPanel
    {
        void SetHighlight();
        void UnsetHighlight();
    }

    public abstract class SettingsPanelRow : Panel, ISettingsPanelRow
    {
        private readonly Color _defaultColor;
        private readonly Color _highlightColor;

        protected SettingsPanelRow(ColorManager colorManager, SpriteFont font, string title) : base(colorManager)
        {
            var titleLabel = new Label(colorManager, font)
            {
                Text = title,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Margin(10f, 0f, 0f, 0f)
            };

            _defaultColor = new Color(120, 154, 255);
            _highlightColor = new Color(120, 154, 227);

            BackgroundColor = _defaultColor;

            AddChild(titleLabel);
        }

        public void SetHighlight()
        {
            BackgroundColor = _defaultColor;
        }

        public void UnsetHighlight()
        {
            BackgroundColor = _highlightColor;
        }
    }
}