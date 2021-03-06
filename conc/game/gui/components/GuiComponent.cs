﻿using System;
using System.Collections.Generic;
using conc.game.commands;
using conc.game.extensions;
using conc.game.input;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui.components
{
    public interface IGuiComponent
    {
        event Action<Command> OnExecuteCommand;
        event Action OnMouseOver;
        event Action OnMouseDown; 
        event Action OnMouseLeave; 

        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);

        IGuiComponent Parent { get; set; }

        Vector2 Position { get; set; }
        Vector2 Size { get; set; }

        float Alpha { get; set; }

        void AddChild(IGuiComponent child);

        VerticalAlignment VerticalAlignment { get; set; }
        HorizontalAlignment HorizontalAlignment { get; set; }
        Margin Margin { get; set; }

        Color BackgroundColor { get; set; }
        Color ForegroundColor { get; set; }

        Rectangle Bounds { get; }

        void OnResize();
    }

    public abstract class GuiComponent : IGuiComponent
    {
        public event Action<Command> OnExecuteCommand;
        public event Action OnMouseOver;
        public event Action OnMouseDown;
        public event Action OnMouseLeave;

        private Vector2 _position;
        private readonly IList<IGuiComponent> _children;
        private Vector2 _size;
        protected readonly InputManager _inputManager;
        private bool _mouseOver;

        protected GuiComponent(InputManager inputManager)
        {
            _inputManager = inputManager;
            _children = new List<IGuiComponent>();
            Margin = new Margin(0f, 0f, 0f, 0f);
            BackgroundColor = Color.White;
            ForegroundColor = Color.Black;
            Alpha = 1f;
        }

        protected virtual void MouseOver(Point mousePosition) { }
        protected virtual void MouseDown(Point mousePosition) { }
        protected virtual void MouseLeave() { }

        protected void ExecuteCommand(Command command)
        {
            OnExecuteCommand?.Invoke(command);
        }

        public virtual void Update(GameTime gameTime)
        {
            var mousePosition = _inputManager.GetMousePosition();

            if (_mouseOver && !Bounds.Intersects(mousePosition))
            {
                _mouseOver = false;
                MouseLeave();
                OnMouseLeave?.Invoke();
            }

            if (Bounds.Intersects(mousePosition))
            {
                _mouseOver = true;
                MouseOver(mousePosition);
                OnMouseOver?.Invoke();
                if (_inputManager.IsPressed(MouseKeys.MouseLeft))
                {
                    MouseDown(mousePosition);
                    OnMouseDown?.Invoke();
                }
            }

            foreach (var child in _children)
                child.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (var child in _children)
                child.Draw(spriteBatch);
        }

        public IGuiComponent Parent { get; set; }

        public Vector2 Position
        {
            get => Parent != null ? new Vector2(Parent.Position.X + _position.X, Parent.Position.Y + _position.Y) : _position;
            set => _position = value;
        }

        public Vector2 Size
        {
            get => _size;
            set
            {
                if (_size == value)
                    return;

                _size = value;
                OnResize();
            }
        }

        public float Alpha { get; set; }

        public void AddChild(IGuiComponent child)
        {
            child.Parent = this;
            child.OnExecuteCommand += ExecuteCommand;
            _children.Add(child);
        }

        public VerticalAlignment VerticalAlignment { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; }
        public Margin Margin { get; set; }
        public Color BackgroundColor { get; set; }
        public Color ForegroundColor { get; set; }

        public Rectangle Bounds => new Rectangle((int)AlignedPosition.X, (int)AlignedPosition.Y, (int)Size.X, (int)Size.Y);

        public virtual void OnResize() { }

        protected Vector2 AlignedPosition
        {
            get
            {
                if (Parent == null)
                    return Position;

                float positionX;
                float positionY;

                switch (HorizontalAlignment)
                {
                    case HorizontalAlignment.None:
                        positionX = Position.X;
                        break;
                    case HorizontalAlignment.Left:
                        positionX = Position.X + Margin.Left;
                        break;
                    case HorizontalAlignment.Center:
                        positionX = Position.X + Parent.Size.X / 2f - Size.X / 2f;
                        break;
                    case HorizontalAlignment.Right:
                        positionX = Position.X + Parent.Size.X - Size.X - Margin.Right;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (VerticalAlignment)
                {
                    case VerticalAlignment.None:
                        positionY = Position.Y;
                        break;
                    case VerticalAlignment.Top:
                        positionY = Position.Y + Margin.Top;
                        break;
                    case VerticalAlignment.Center:
                        positionY = Position.Y + Parent.Size.Y / 2f - Size.Y / 2f;
                        break;
                    case VerticalAlignment.Bottom:
                        positionY = Position.Y + Parent.Size.Y - Size.Y - Margin.Bottom;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return new Vector2(positionX, positionY);
            }
        }
    }

    public enum VerticalAlignment
    {
        None,
        Top,
        Center,
        Bottom
    }

    public enum HorizontalAlignment
    {
        None,
        Left,
        Center,
        Right
    }

    public class Margin
    {
        public Margin(float left, float top, float right, float bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public float Left { get; }
        public float Top { get; }
        public float Right { get; }
        public float Bottom { get; }
    }
}