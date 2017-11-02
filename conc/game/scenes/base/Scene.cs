using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.scenes.@base
{
    public interface IScene
    {
        void LoadContent(ContentManager contentManager);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

    public abstract class Scene : IScene
    {
        protected Scene(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        protected GraphicsDevice GraphicsDevice { get; }

        public virtual void LoadContent(ContentManager contentManager) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}