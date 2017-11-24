using System;
using System.Collections.Generic;
using conc.game.commands;
using conc.game.extensions;
using conc.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.scenes.baseclass
{
    public interface IMenu
    {
        event Action<Command> ExecuteCommand;

        void LoadContent(ContentManager contentManager, Tuple<string, Command>[] items);
        void Update(InputManager inputManager);
        void Draw(SpriteBatch spriteBatch);

        IList<MenuItem> Items { get; }
    }

    public class Menu : IMenu
    {
        public event Action<Command> ExecuteCommand;
        
        private Vector2 _position;
        private int _index;
        private SpriteFont _font;
        public IList<MenuItem> Items { get; private set; }

        public Menu(Vector2 position)
        {
            _position = position;
        }

        public void LoadContent(ContentManager contentManager, Tuple<string, Command>[] items)
        {
            _font = contentManager.Load<SpriteFont>("fonts/menu");
            Items = new List<MenuItem>();

            for (var i = 0; i < items.Length; i++)
            {
                var text = items[i].Item1;
                var command = items[i].Item2;
                var pos = new Vector2(_position.X, _position.Y + i * 30);
                var size = _font.MeasureString(text);

                Items.Add(new MenuItem(text, pos, size, command));
            }
        }

        public void Update(InputManager inputManager)
        {
            foreach (var menuItem in Items)
            {
                if (menuItem.HasContent() && menuItem.Bounds.Intersects(inputManager.GetMousePosition()))
                {
                    if (inputManager.IsPressed(ControlButtons.FireRope, 0))
                        ExecuteCommand?.Invoke(menuItem.Command);

                    menuItem.Highlight = true;
                }
                else
                    menuItem.Highlight = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var menuItem in Items)
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
}