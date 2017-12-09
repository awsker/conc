using System;
using System.Collections.Generic;
using conc.game.commands;
using conc.game.extensions;
using conc.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace conc.game.scenes.baseclass
{
    public interface IMenu
    {
        event Action<Command> ExecuteCommand;

        void LoadContent(ContentManager contentManager, Tuple<string, Command>[] items);
        void Update(InputManager inputManager);
        void Draw(SpriteBatch spriteBatch);
    }

    public class Menu : IMenu
    {
        public event Action<Command> ExecuteCommand;
        
        private Vector2 _position;
        private int _index;
        private ControlMode _controlMode;
        private Point _previousMousePosition;
        private bool _keyboardActivated;

        private SpriteFont _font;
        private IList<MenuItem> _items;

        public Menu(Vector2 position)
        {
            _position = position;
            _controlMode = ControlMode.Keyboard;
        }

        public void LoadContent(ContentManager contentManager, Tuple<string, Command>[] items)
        {
            _font = contentManager.Load<SpriteFont>("fonts/menu");
            _items = new List<MenuItem>();

            for (var i = 0; i < items.Length; i++)
            {
                var text = items[i].Item1;
                var command = items[i].Item2;
                var pos = new Vector2(_position.X, _position.Y + i * 30);
                var size = _font.MeasureString(text);

                _items.Add(new MenuItem(text, pos, size, command));
            }
        }

        private void IncreaseIndex()
        {
            _index++;
            if (_index >= _items.Count)
                _index = 0;
        }

        private void DecreaseIndex()
        {
            _index--;
            if (_index < 0)
                _index = _items.Count - 1;
        }

        public void Update(InputManager inputManager)
        {
            var mousePosition = inputManager.GetMousePosition();
            var nextKeyPress = inputManager.GetNextKeyPress();

            if (mousePosition != _previousMousePosition)
            {
                _controlMode = ControlMode.Mouse;
                ExecuteCommand?.Invoke(new Command(CommandType.ShowCursor));
            }
            else if (nextKeyPress != Keys.None && nextKeyPress != Keys.Escape && nextKeyPress != Keys.Delete && !inputManager.IsDown(MouseKeys.MouseLeft))
            {
                if (_controlMode == ControlMode.Mouse)
                    _keyboardActivated = true;

                _controlMode = ControlMode.Keyboard;
                ExecuteCommand?.Invoke(new Command(CommandType.HideCursor));
            }

            if (!_keyboardActivated)
            {
                if (inputManager.IsPressed(ControlButtons.MoveUp, 0))
                {
                    DecreaseIndex();
                    while (!_items[_index].HasContent())
                        DecreaseIndex();
                }
                else if (inputManager.IsPressed(ControlButtons.MoveDown, 0))
                {
                    IncreaseIndex();
                    while (!_items[_index].HasContent())
                        IncreaseIndex();
                }
            }

            foreach (var menuItem in _items)
            {
                menuItem.Highlight = false;

                if (_controlMode == ControlMode.Mouse)
                {
                    if (menuItem.HasContent() && menuItem.Bounds.Intersects(mousePosition))
                    {
                        if (inputManager.IsPressed(ControlButtons.FireRope, 0) || inputManager.IsPressed(MouseKeys.MouseLeft))
                            ExecuteCommand?.Invoke(menuItem.Command);

                        menuItem.Highlight = true;
                    }
                }
            }

            if (_controlMode == ControlMode.Keyboard)
            {
                if (inputManager.IsPressed(ControlButtons.FireRope, 0))
                    ExecuteCommand?.Invoke(_items[_index].Command);

                _items[_index].Highlight = true;
            }

            _previousMousePosition = mousePosition;
            _keyboardActivated = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var menuItem in _items)
                spriteBatch.DrawString(_font, menuItem.Text, menuItem.Position, menuItem.Highlight ? Color.Bisque : Color.White);
        }
    }

    public class MenuItem
    {
        private Vector2 _size;

        public MenuItem(string text, Vector2 position, Vector2 size, Command command)
        {
            Text = text;
            Position = position;
            _size = size;
            Command = command;
        }

        public string Text { get; }
        public Vector2 Position { get; }
        public Command Command { get; }
        public bool Highlight { get; set; }

        public bool HasContent()
        {
            return Text != string.Empty;
        }

        public Rectangle Bounds => new Rectangle((int) Position.X, (int) Position.Y, (int) _size.X, (int) _size.Y);
    }

    public enum ControlMode
    {
        Mouse,
        Keyboard
    }
}