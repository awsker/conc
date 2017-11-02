using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.scenes.@base
{
    public interface IScene
    {
        void SetGameManager(IGameManager manager);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

    public abstract class Scene : IScene
    {
        public virtual void SetGameManager(IGameManager manager) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}