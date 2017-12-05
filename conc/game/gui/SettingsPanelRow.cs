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
        Rectangle FocusBounds { get; }
        void Activate();
        void Deactivate();
        bool Activated { get; }
    }

    public abstract class SettingsPanelRow : Panel, ISettingsPanelRow
    {
        private readonly Color _defaultColor;
        private readonly Color _highlightColor;
        protected bool _activated;

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

        public abstract Rectangle FocusBounds { get; }

        public void Activate()
        {
            _activated = true;
        }

        public void Deactivate()
        {
            _activated = false;
        }

        public bool Activated => _activated;
    }
}