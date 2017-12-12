using System;
using conc.game.commands;
using conc.game.extensions;
using conc.game.gui.components;
using conc.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace conc.game.gui
{
    public class KeybindRow : SettingsPanelRow
    {
        private readonly ILabel _keyLabel1;
        private readonly ILabel _keyLabel2;

        private string _previousKey1Text;
        private string _previousKey2Text;

        private readonly Color _defaultKeyBackground;
        private readonly Color _keyHighlightBackground;

        private bool _key1Activated;
        private bool _key2Activated;

        private bool _key1Highlighted;
        private bool _key2Highlighted;

        private ControlButtons _controlButton;

        public KeybindRow(InputManager inputManager, SpriteFont font, ControlButtons controlButton, GenericKey key1, GenericKey key2) : base(inputManager, font, controlButton.ToString())
        {
            _previousKey1Text = "<none>";
            _previousKey2Text = "<none>";

            _defaultKeyBackground = new Color(105, 143, 240);
            _keyHighlightBackground = new Color(105, 120, 250);

            _keyLabel1 = new Label(inputManager, font)
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

            _keyLabel1.OnMouseOver += KeyLabel1OnOnMouseOver;
            _keyLabel1.OnMouseLeave += KeyLabel1OnOnMouseLeave;

            _keyLabel2 = new Label(inputManager, font)
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

            _keyLabel2.OnMouseOver += KeyLabel2OnOnMouseOver;
            _keyLabel2.OnMouseLeave += KeyLabel2OnOnMouseLeave;

            _controlButton = controlButton;
            
            SetKey1(key1?.Text);
            SetKey2(key2?.Text);
            
            AddChild(_keyLabel1);
            AddChild(_keyLabel2);
        }

        private void KeyLabel1OnOnMouseOver()
        {
            _keyLabel1.BackgroundColor = _keyHighlightBackground;
            _key1Highlighted = true;
        }

        private void KeyLabel2OnOnMouseOver()
        {
            _keyLabel2.BackgroundColor = _keyHighlightBackground;
            _key2Highlighted = true;
        }

        private void KeyLabel1OnOnMouseLeave()
        {
            _keyLabel1.BackgroundColor = _defaultKeyBackground;
            _key1Highlighted = false;
        }

        private void KeyLabel2OnOnMouseLeave()
        {
            _keyLabel2.BackgroundColor = _defaultKeyBackground;
            _key2Highlighted = false;
        }

        protected override void MouseLeave()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            var nextKey = _inputManager.GetNextKeyPress();
            var nextMouseKey = _inputManager.GetNextMouseKeyPress();

            if (nextKey == Keys.Delete && _key1Highlighted)
            {
                SetKey1(string.Empty);
                ExecuteCommand(new Command(_controlButton, Key1Text, Key2Text));
                return;
            }

            if (nextKey == Keys.Delete && _key2Highlighted)
            {
                SetKey2(string.Empty);
                ExecuteCommand(new Command(_controlButton, Key1Text, Key2Text));
                return;
            }

            if (!Activated)
            {
                if (_inputManager.IsMouseDownOverBounds(Key1Bounds))
                {
                    Key1Activated = true;
                    SetKey1Text("press key");
                    return;
                }
                if (_inputManager.IsMouseDownOverBounds(Key2Bounds))
                {
                    Key2Activated = true;
                    SetKey2Text("press key");
                    return;
                }
            }

            if (Activated)
            {
                if (nextKey == Keys.Escape)
                {
                    Key1Activated = false;
                    Key2Activated = false;
                    return;
                }

                if (Activated && (nextKey != Keys.None || nextMouseKey != MouseKeys.None))
                {
                    var keyText = string.Empty;
                    if (nextKey != Keys.None)
                        keyText = nextKey.ToString();
                    if (nextMouseKey != MouseKeys.None)
                        keyText = nextMouseKey.ToString();

                    if (Key1Activated)
                        SetKey1(keyText);
                    else if (Key2Activated)
                        SetKey2(keyText);

                    Key1Activated = false;
                    Key2Activated = false;

                    ExecuteCommand(new Command(_controlButton, Key1Text, Key2Text));
                }
            }

            base.Update(gameTime);
        }

        private void SetKey1(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                _keyLabel1.Text = "<none>";
                return;
            }
            
            _keyLabel1.Text = key;
            _previousKey1Text = _keyLabel1.Text;
        }

        private void SetKey2(string key)
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

        private void SetKey1Text(string text)
        {
            if (_keyLabel1.Text != text)
                _keyLabel1.Text = text;
        }

        private void SetKey2Text(string text)
        {
            if (_keyLabel2.Text != text)
                _keyLabel2.Text = text;
        }

        public Rectangle Key1Bounds => _keyLabel1.Bounds;
        public Rectangle Key2Bounds => _keyLabel2.Bounds;

        //public void SetKey1Highlight()
        //{
        //    _keyLabel1.BackgroundColor = _keyHighlightBackground;
        //    _key1Highlighted = true;
        //}

        //public void SetKey2Highlight()
        //{
        //    _keyLabel2.BackgroundColor = _keyHighlightBackground;
        //    _key2Highlighted = true;
        //}

        //public void UnsetKeyHighlights()
        //{
        //    _keyLabel1.BackgroundColor = _defaultKeyBackground;
        //    _keyLabel2.BackgroundColor = _defaultKeyBackground;

        //    _key1Highlighted = false;
        //    _key2Highlighted = false;
        //}

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

        public override Rectangle FocusBounds => Bounds;
        public override bool Activated => Key1Activated || Key2Activated;
    }
}