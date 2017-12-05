using conc.game.gui.components;
using conc.game.util;
using Microsoft.Xna.Framework.Graphics;
using OpenTK.Input;

namespace conc.game.gui
{
    public interface IKeybindRow : ISettingsPanelRow
    {
        void SetKey(Key? key);
    }

    public class KeybindRow : SettingsPanelRow, IKeybindRow
    {
        private Key? _key;
        private readonly ILabel _keyLabel;

        public KeybindRow(ColorManager colorManager, SpriteFont font, string title) : base(colorManager, font, title)
        {
            _keyLabel = new Label(colorManager, font)
            {
                Text = "<none>",
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Margin(0f, 0f, 10f, 0f)
            };

            AddChild(_keyLabel);
        }

        public void SetKey(Key? key)
        {
            _key = key;

            if (_key == null)
            {
                _keyLabel.Text = "<none>";
                return;
            }
            
            _keyLabel.Text = _key.ToString();
        }
    }
}