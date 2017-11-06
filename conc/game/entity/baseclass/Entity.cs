using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.entity.baseclass
{
    public interface IEntity
    {
        Vector2 Velocity { get; set; }
        Transform Transform { get; }
        void LoadContent(ContentManager contentManager);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        bool IsVisible { get; }
        bool IsDestroyed { get; }
    }

    public abstract class Entity : IEntity
    {
        protected Entity(Vector2 position = new Vector2())
        {
            Transform = new Transform {Position = position};
        }

        public virtual void Destroy()
        {
            IsDestroyed = true;
        }

        public Transform Transform { get; }
        public Vector2 Velocity { get; set; }

        public Vector2 Position
        {
            get => Transform.Position;
            set => Transform.Position = value;
        }

        public Vector2 Scale
        {
            get => Transform.Scale;
            set => Transform.Scale = value;
        }

        public float Rotation
        {
            get => Transform.Rotation;
            set => Transform.Rotation = value;
        }

        public virtual bool IsVisible => true;
        public virtual bool IsDestroyed { get; set; }
        public virtual void LoadContent(ContentManager contentManager) { }

        public virtual void Update(GameTime gameTime)
        {}

        public virtual void Draw(SpriteBatch spriteBatch)
        {}
    }
}