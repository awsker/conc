using conc.game.util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.gui.components
{
    public interface IPanel : IGuiComponent
    {
    }

    public class Panel : GuiComponent, IPanel
    {
        public Panel(ColorManager colorManager) : base(colorManager)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (BackgroundColor != null)
                spriteBatch.Draw(_colorManager.Get(BackgroundColor), AlignedPosition, null, Color.White*Alpha, 0f, Vector2.Zero, Size, SpriteEffects.None, 0f);

            base.Draw(spriteBatch);
        }
    }
}