using conc.game.gui.components;
using conc.game.input;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace conc.game.gui
{
    public interface IKeybindRow : ISettingsPanelRow
    {
        void SetKey(Keys? key);
        void SetText(string text);
        ControlButtons ControlButton { get; }
    }

    public class KeybindRow : SettingsPanelRow, IKeybindRow
    {
        private Keys? _key;
        private readonly ILabel _keyLabel;

        private string _previousText;

        public KeybindRow(ColorManager colorManager, SpriteFont font, ControlButtons controlButton, Keys key) : base(colorManager, font, controlButton.ToString())
        {
            _previousText = "<none>";

            _keyLabel = new Label(colorManager, font)
            {
                Text = "<none>",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Margin(0f, 0f, 10f, 0f)
            };

            ControlButton = controlButton;
            SetKey(key);

            AddChild(_keyLabel);
        }

        public void SetKey(Keys? key)
        {
            _key = key;

            if (_key == null)
            {
                _keyLabel.Text = "<none>";
                return;
            }
            
            _keyLabel.Text = _key.ToString();
            _previousText = _keyLabel.Text;
        }

        public void SetText(string text)
        {
            if (_keyLabel.Text != text)
                _keyLabel.Text = text;
        }

        public override void Deactivate()
        {
            _keyLabel.Text = _previousText;
            base.Deactivate();
        }

        public ControlButtons ControlButton { get; }

        public override Rectangle FocusBounds => Bounds;
    }
}