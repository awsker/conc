using System;
using conc.game.input;
using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui.components
{
    public interface ILabel : IPanel
    {
        string Text { get; set; }
        TextHorizontalAlignment TextHorizontalAlignment { get; set; }
        TextVerticalAlignment TextVerticalAlignment { get; set; }
        Margin TextMargin { get; set; }
    }

    public class Label : Panel, ILabel
    {
        private readonly SpriteFont _font;
        private string _text;
        private Vector2 _textSize;

        public Label(InputManager inputManager, SpriteFont font) : base(inputManager)
        {
            _font = font;
            ForegroundColor = Color.White;
            Alpha = 0f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            
            spriteBatch.DrawString(_font, Text, TextPosition, ForegroundColor);
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                _textSize = _font.MeasureString(value);
            }
        }

        public TextHorizontalAlignment TextHorizontalAlignment { get; set; }
        public TextVerticalAlignment TextVerticalAlignment { get; set; }
        public Margin TextMargin { get; set; }

        private Vector2 TextPosition
        {
            get
            {
                float positionX;
                float positionY;

                switch (TextHorizontalAlignment)
                {
                    case TextHorizontalAlignment.Left:
                        positionX = AlignedPosition.X + Margin.Left;
                        break;
                    case TextHorizontalAlignment.Center:
                        positionX = AlignedPosition.X + Size.X / 2f - _textSize.X / 2f;
                        break;
                    case TextHorizontalAlignment.Right:
                        positionX = AlignedPosition.X + Size.X - _textSize.X - Margin.Right;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                switch (TextVerticalAlignment)
                {
                    case TextVerticalAlignment.Top:
                        positionY = AlignedPosition.Y + Margin.Top;
                        break;
                    case TextVerticalAlignment.Center:
                        positionY = AlignedPosition.Y + Size.Y / 2f - _textSize.Y / 2f;
                        break;
                    case TextVerticalAlignment.Bottom:
                        positionY = AlignedPosition.Y + Size.Y - _textSize.Y - Margin.Bottom;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                return new Vector2(positionX, positionY);
            }
        }
    }

    public enum TextHorizontalAlignment
    {
        Left,
        Center,
        Right
    }

    public enum TextVerticalAlignment
    {
        Top,
        Center,
        Bottom
    }
}