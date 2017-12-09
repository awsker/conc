using System;
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
        void SetKey1(string key);
        void SetKey2(string key);

        string Key1Text { get; }
        string Key2Text { get; }

        void SetKey1Text(string text);
        void SetKey2Text(string text);
        ControlButtons ControlButton { get; }

        Rectangle Key1Bounds { get; }
        Rectangle Key2Bounds { get; }

        void SetKey1Highlight();
        void SetKey2Highlight();
        void UnsetKeyHighlights();

        bool Key1Activated { get; set; }
        bool Key2Activated { get; set; }

        bool Key1Highlighted { get; }
        bool Key2Highlighted { get; }
    }

    public class KeybindRow : SettingsPanelRow, IKeybindRow
    {
        private readonly ILabel _keyLabel1;
        private readonly ILabel _keyLabel2;

        private string _previousKey1Text;
        private string _previousKey2Text;

        private Color _defaultKeyBackground;
        private Color _keyHighlightBackground;

        private bool _key1Activated;
        private bool _key2Activated;

        private bool _key1Highlighted;
        private bool _key2Highlighted;

        public KeybindRow(ColorManager colorManager, SpriteFont font, ControlButtons controlButton, GenericKey key1, GenericKey key2) : base(colorManager, font, controlButton.ToString())
        {
            _previousKey1Text = "<none>";
            _previousKey2Text = "<none>";

            _defaultKeyBackground = new Color(105, 143, 240);
            _keyHighlightBackground = new Color(105, 120, 250);

            _keyLabel1 = new Label(colorManager, font)
            {
                Text = "<none>",
                Size = new Vector2(150, 32),
                BackgroundColor = _defaultKeyBackground,
                Alpha = 1f,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Margin(0f, 2f, 158f, 0f),
                TextHorizontalAlignment = TextHorizontalAlignment.Center,
                TextVerticalAlignment = TextVerticalAlignment.Center
            };

            _keyLabel2 = new Label(colorManager, font)
            {
                Text = "<none>",
                Size = new Vector2(150, 32),
                BackgroundColor = _defaultKeyBackground,
                Alpha = 1f,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Margin(0f, 2f, 4f, 0f),
                TextHorizontalAlignment = TextHorizontalAlignment.Center,
                TextVerticalAlignment = TextVerticalAlignment.Center
            };

            ControlButton = controlButton;
            
            SetKey1(key1?.Text);
            SetKey2(key2?.Text);
            
            AddChild(_keyLabel1);
            AddChild(_keyLabel2);
        }

        public void SetKey1(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _keyLabel1.Text = "<none>";
                return;
            }
            
            _keyLabel1.Text = key;
            _previousKey1Text = _keyLabel1.Text;
        }

        public void SetKey2(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _keyLabel2.Text = "<none>";
                return;
            }

            _keyLabel2.Text = key;
            _previousKey2Text = _keyLabel2.Text;
        }

        public string Key1Text => _keyLabel1.Text;
        public string Key2Text => _keyLabel2.Text;

        public void SetKey1Text(string text)
        {
            if (_keyLabel1.Text != text)
                _keyLabel1.Text = text;
        }

        public void SetKey2Text(string text)
        {
            if (_keyLabel2.Text != text)
                _keyLabel2.Text = text;
        }

        public ControlButtons ControlButton { get; }

        public Rectangle Key1Bounds => _keyLabel1.Bounds;
        public Rectangle Key2Bounds => _keyLabel2.Bounds;

        public void SetKey1Highlight()
        {
            _keyLabel1.BackgroundColor = _keyHighlightBackground;
            _key1Highlighted = true;
        }

        public void SetKey2Highlight()
        {
            _keyLabel2.BackgroundColor = _keyHighlightBackground;
            _key2Highlighted = true;
        }

        public void UnsetKeyHighlights()
        {
            _keyLabel1.BackgroundColor = _defaultKeyBackground;
            _keyLabel2.BackgroundColor = _defaultKeyBackground;

            _key1Highlighted = false;
            _key2Highlighted = false;
        }

        public bool Key1Activated
        {
            get => _key1Activated;
            set
            {
                _key1Activated = value;
                _keyLabel1.Text = _previousKey1Text;
            }
        }

        public bool Key2Activated
        {
            get => _key2Activated;
            set
            {
                _key2Activated = value;
                _keyLabel2.Text = _previousKey2Text;
            }
        }

        public bool Key1Highlighted => _key1Highlighted;
        public bool Key2Highlighted => _key2Highlighted;

        public override Rectangle FocusBounds => Bounds;
        public override bool Activated => Key1Activated || Key2Activated;
    }
}