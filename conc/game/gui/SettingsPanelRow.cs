using conc.game.gui.components;
using conc.game.input;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui
{
    public interface ISettingsPanelRow : IPanel
    {
        void SetHighlight();
        void UnsetHighlight();
        Rectangle FocusBounds { get; }
        bool Activated { get; }
    }

    public abstract class SettingsPanelRow : Panel, ISettingsPanelRow
    {
        private readonly Color _defaultColor;
        private readonly Color _highlightColor;

        protected SettingsPanelRow(InputManager inputManager, SpriteFont font, string title) : base(inputManager)
        {
            var titleLabel = new Label(inputManager, font)
            {
                Text = title,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Margin(4f, 0f, 0f, 0f),
                TextHorizontalAlignment = TextHorizontalAlignment.Left,
                TextVerticalAlignment = TextVerticalAlignment.Center
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

        public abstract Rectangle FocusBounds { get; }

        public virtual bool Activated => false;
    }
}