using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.entity.baseclass
{
    public interface IEntity
    {
        Transform Transform { get; }
        void LoadContent(ContentManager contentManager);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
    }

    public abstract class Entity : IEntity
    {
        protected Entity(Vector2 position = new Vector2())
        {
            Transform = new Transform {Position = position};
        }

        public Transform Transform { get; }
        public virtual void LoadContent(ContentManager contentManager) { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}