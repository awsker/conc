using conc.game.math;
using conc.game.scenes.baseclass;
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
        void Destroy();
        bool IsVisible { get; }
        bool IsDestroyed { get; }
        int Width { get; }
        int Height { get; }
        Point Offset { get; }
        Box BoundingBox { get; }
        IScene Scene { get; set; }
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

        public virtual int Width => 0;
        public virtual int Height => 0;
        public virtual Point Offset => Point.Zero;

        public virtual Box BoundingBox => new Box(Width, Height) { Scale = Scale, Origin = new Vector2(Offset.X, Offset.Y), Position = Position, Rotation = Rotation };

        public virtual IScene Scene { get; set; }
    }
}