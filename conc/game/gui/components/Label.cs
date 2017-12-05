using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui.components
{
    public interface ILabel : IGuiComponent
    {
        string Text { get; set; }
    }

    public class Label : GuiComponent, ILabel
    {
        private readonly SpriteFont _font;
        private string _text;

        public Label(ColorManager colorManager, SpriteFont font) : base(colorManager)
        {
            _font = font;
            ForegroundColor = Color.White;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (Parent == null)
                spriteBatch.DrawString(_font, Text, Position, ForegroundColor);
            else
            {
                spriteBatch.DrawString(_font, Text, AlignedPosition, ForegroundColor);
            }
            

            base.Draw(spriteBatch);
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                Size = _font.MeasureString(value);
            }
        }
    }
}