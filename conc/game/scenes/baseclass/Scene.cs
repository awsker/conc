using System.Collections.Generic;
using System.Collections.ObjectModel;
using conc.game.entity.baseclass;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace conc.game.scenes.baseclass
{
    public interface IScene
    {
        IGameManager GameManager { get; set; }
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void AddEntity(IEntity entity);
    }

    public abstract class Scene : IScene
    {
        protected IList<IEntity> _entities;
        public Scene()
        {
            _entities = new List<IEntity>();
        }

        protected IReadOnlyCollection<IEntity> Entities => new ReadOnlyCollection<IEntity>(_entities);

        public virtual void AddEntity(IEntity entity)
        {
            _entities.Add(entity);
            entity.Scene = this;
        }
        public virtual IGameManager GameManager { get; set; }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }
}